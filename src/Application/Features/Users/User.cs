using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.Users;

public class User : BaseEntity, IHasDomainEvent
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


    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
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
    public bool Active { get; set; }
}

public class UserCompletedEvent : DomainEvent
{
    public UserCompletedEvent(User item)
    {
        Item = item;
    }

    public User Item { get; }
}

