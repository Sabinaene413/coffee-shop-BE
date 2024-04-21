using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Users
{
    public class UserCredentialsConfiguration : IEntityTypeConfiguration<UserCredential>
    {

        public void Configure(EntityTypeBuilder<UserCredential> builder)
        {
            builder.HasNoDiscriminator();
            builder.Property(p => p.Id);

            builder.HasData(new UserCredential()
            {
                Id = 1,
                Email = UserCredential.DefaultAdminEmail,
                PasswordHash = PasswordUtils.Hash(UserCredential.DefaultPassword, out var passwordSalt),
                PasswordSalt = passwordSalt,
            });
        }
    }
}
