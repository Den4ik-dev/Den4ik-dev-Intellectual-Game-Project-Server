using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Models;

namespace server.Domain.Database.EntityTypeConfigurations;

internal class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable("images");

        builder.Property(img => img.Id).HasColumnName("image_id");
        builder.Property(img => img.Path).HasColumnName("path");
        builder.Property(img => img.QuestionId).HasColumnName("question_id");
    }
}
