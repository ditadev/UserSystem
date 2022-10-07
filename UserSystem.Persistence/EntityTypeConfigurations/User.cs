using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserService.Persistence.EntityTypeConfigurations;

public class User : IEntityTypeConfiguration<UserSystem.Models.User>
{
    public void Configure(EntityTypeBuilder<UserSystem.Models.User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.PhoneNumber).IsRequired(false);
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired(false);
    }
}