using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserSystem.Models.Enums;

namespace UserService.Persistence.EntityTypeConfigurations;

public class Role : IEntityTypeConfiguration<UserSystem.Models.Role>
{
    public void Configure(EntityTypeBuilder<UserSystem.Models.Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasMany(r => r.Users)
            .WithMany(u => u.Roles);
        builder.HasData(new object[]
        {
            new UserSystem.Models.Role { Id = UserRole.User },
            new UserSystem.Models.Role { Id = UserRole.Administrator }
        });
    }
}