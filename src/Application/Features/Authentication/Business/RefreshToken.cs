using MediatR;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Services;
using System.Security.Claims;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Users;

namespace MyCoffeeShop.Application.Features.Authentications.Business;

public record RefreshTokenCommand(string AccessToken, string RefreshToken)
    : IRequest<RefreshTokenResponse>;

public record RefreshTokenResponse(string accessToken, string refreshToken);

internal sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly TokenService _tokenService;

    public RefreshTokenCommandHandler(
        ApplicationDbContext applicationDbContext,
        TokenService tokenService
    )
    {
        _applicationDbContext = applicationDbContext;
        _tokenService = tokenService;
    }

    public async Task<RefreshTokenResponse> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        // TODO: Handle invalid token and purging
        if (request is null)
        {
            throw new NotFoundException("Invalid client request");
        }

        string? accessToken = request.AccessToken;
        string? refreshToken = request.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            throw new NotFoundException("Invalid access token or refresh token");
        }
        var identity = principal.Identity as ClaimsIdentity;
        var userCredentialId = (principal.Identity as ClaimsIdentity)
            ?.FindFirst(SystemClaimType.UCID)
            ?.Value;
        var userId = (principal.Identity as ClaimsIdentity)?.FindFirst(SystemClaimType.UID)?.Value;

        if (userCredentialId == null || userId == null)
        {
            throw new NotFoundException("Invalid access token or refresh token");
        }
        var userCredential = await _applicationDbContext.UserCredentials.FirstOrDefaultAsync(
                                x => x.Id == long.Parse(userCredentialId), cancellationToken
                            ) ?? throw new NotFoundException(nameof(User), userId);

        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(
                                x => x.UserCredentialId == userCredential.Id, cancellationToken
                            ) ?? throw new NotFoundException(nameof(User), userId);
        if (
            userCredential.RefreshToken != refreshToken
            || userCredential.RefreshTokenExpiryTime <= DateTime.Now
        )
        {
            throw new NotFoundException("Invalid access token or refresh token");
        }

        var newAccessToken = _tokenService.CreateToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        userCredential.RefreshToken = newRefreshToken;
        userCredential.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_tokenService._tokenConfigurations.ExpirationMinutes);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponse(newAccessToken, newRefreshToken);
    }
}
