using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using MyCoffeeShop.Application.Users;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Services;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.CoffeeShops;

namespace MyCoffeeShop.Application.Features.Authentications.Business;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

public class LoginResponse
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public UserRole Role { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public LoginResponse(long id, string? firstName, string? lastName, string? userName, string? email, UserRole role, string accessToken, string refreshToken)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Email = email;
        Role = role;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.Email).NotEmpty();
        RuleFor(v => v.Password).NotEmpty();
    }

    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public LoginCommandHandler(
             ApplicationDbContext applicationDbContext,
            IMapper mapper,
            TokenService tokenService,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<LoginResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken
        )
        {
            var userCredential = await _applicationDbContext.UserCredentials.AsNoTracking()
                            .Where(x => string.Compare(x.Email, request.Email) == 0)
                            .OrderByDescending(x => x.Id).FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException(nameof(User), request.Email);

            if (
                !PasswordUtils.Verify(
                    request.Password,
                    userCredential.PasswordHash,
                    userCredential.PasswordSalt
                )
            )
            {
                throw new Exception("Parola nu se potriveste.");
            }

            var user = await _applicationDbContext.Users.AsNoTracking()
                            .FirstOrDefaultAsync(x => string.Compare(x.Email, request.Email) == 0, cancellationToken) ??
                throw new NotFoundException(nameof(User), request.Email);

            var locationDto = user.LocationId.HasValue ? _mapper.Map<CoffeeShopDto>(await _applicationDbContext.CoffeeShops.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == user.LocationId, cancellationToken)) : null;

            var accessToken = _tokenService.CreateToken(user, locationDto);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenValidityInDays = Int32.Parse(
                _configuration["TokenConfig:RefreshTokenValidityInDays"]
            );

            userCredential.RefreshToken = refreshToken;
            userCredential.RefreshTokenExpiryTime = DateTime.Now.AddDays(
                refreshTokenValidityInDays
            );

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new LoginResponse(user.Id,
                                     user.Email,
                                     user.FirstName,
                                     user.LastName,
                                     user.UserName,
                                     user.Role,
                                     accessToken,
                                     refreshToken);
        }
    }
}
