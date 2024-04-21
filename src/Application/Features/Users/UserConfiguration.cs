using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoffeeShop.Application.Common.Constants;

namespace MyCoffeeShop.Application.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(t => t.FirstName)
            .HasMaxLength(ConfigurationConstants.nameStringLength);

        builder.Property(t => t.LastName)
            .HasMaxLength(ConfigurationConstants.nameStringLength);

        builder.Property(t => t.Email)
            .HasMaxLength(ConfigurationConstants.nameStringLength)
            .IsRequired();

        builder.HasIndex(builder => builder.Email)
            .IsUnique();

        builder.HasData(new User()
        {
            Id = 1,
            Email = "coffee_shop@gmail.com",
            FirstName = "Admin",
            LastName = "CoffeeShop",
            Role = UserRole.Admin,
            UserName = "admin",
            UserCredentialId = 1
        });
    }
}
