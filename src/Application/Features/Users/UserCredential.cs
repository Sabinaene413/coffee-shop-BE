
using MyCoffeeShop.Application.Common.Abstracts;

namespace MyCoffeeShop.Application.Users;

public class UserCredential : BaseEntity
{
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public const string DefaultPassword = "1234";
    public const string DefaultAdminEmail = "coffee_shop@gmail.com";

}
