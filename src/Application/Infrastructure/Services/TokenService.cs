using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MyCoffeeShop.Application.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyCoffeeShop.Application.Features.Authentications;
using Microsoft.Extensions.Logging;

namespace MyCoffeeShop.Application.Infrastructure.Services;

public class TokenConfigurations
{
    public required string Secret { get; set; }
    public required int ExpirationMinutes { get; set; }
    public required string RefreshTokenValidityInDays { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
}

public class TokenService
{
    private readonly ILogger<TokenService> _logger;
    public required TokenConfigurations _tokenConfigurations;
    public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
    {
        _logger = logger;
#pragma warning disable CS8601 // Possible null reference assignment.
        _tokenConfigurations = configuration?.GetSection("TokenConfig").Get<TokenConfigurations>();
#pragma warning restore CS8601 // Possible null reference assignment.
    }

    public string CreateToken(User user)
    {

        var expiration = DateTime.UtcNow.AddMinutes(_tokenConfigurations.ExpirationMinutes);
        var token = CreateJwtToken(CreateClaims(user), CreateSigningCredentials(), expiration);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(
        List<Claim> claims,
        SigningCredentials credentials,
        DateTime expiration
    )
    {
        string? issuer = _tokenConfigurations.Issuer;
        string? audience = _tokenConfigurations.Audience;

        return new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }

    private List<Claim> CreateClaims(User user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim(SystemClaimType.UID, user.Id.ToString()),
                new Claim(SystemClaimType.UCID, user.UserCredentialId.ToString()),
                new Claim(SystemClaimType.FirstName, user.FirstName ?? ""),
                new Claim(SystemClaimType.LastName, user.LastName ?? ""),
                new Claim(SystemClaimType.Email, user.Email ?? ""),
                new Claim(SystemClaimType.RoleId, user.Role.ToString())
            };
            return claims;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating claims");
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfigurations.Secret)),
            SecurityAlgorithms.HmacSha256
        );
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_tokenConfigurations.Secret)
            ),
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out SecurityToken securityToken
        );
        if (
            securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase
            )
        )
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
