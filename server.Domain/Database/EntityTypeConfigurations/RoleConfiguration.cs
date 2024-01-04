using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Models;

namespace server.Domain.Database.EntityTypeConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasIndex(r => r.Name);
        builder.Property(r => r.Id).HasColumnName("role_id");
        builder.Property(r => r.Name).HasColumnName("name");
    }
}
