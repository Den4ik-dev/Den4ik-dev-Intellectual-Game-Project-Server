using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Models;

namespace server.Domain.Database.EntityTypeConfigurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasIndex(u => u.Login).IsUnique();

        builder.Property(u => u.Id).HasColumnName("user_id");
        builder.Property(u => u.Login).HasColumnName("login");
        builder.Property(u => u.Password).HasColumnName("password");
        builder.Property(u => u.RoleId).HasColumnName("role_id");
        builder.Property(u => u.RefreshToken).HasColumnName("refresh_token");
        builder.Property(u => u.RefreshTokenExpiryTime).HasColumnName("refresh_token_expiry_time");
    }
}
