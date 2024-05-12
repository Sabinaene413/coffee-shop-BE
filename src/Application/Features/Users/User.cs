using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.Users;

public class User : BaseEntity
{
    public User()
    {
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public UserRole Role { get; set; }
    public long UserCredentialId { get; set; }
    public long? LocationId { get; set; }

}

public enum UserRole
{
    User,
    Admin
}

public class UserDto : IMapFrom<User>
{
    public required long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public UserRole Role { get; set; }
    public long? LocationId { get; set; }
}
