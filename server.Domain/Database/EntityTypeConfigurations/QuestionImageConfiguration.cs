using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Models;

namespace server.Domain.Database.EntityTypeConfigurations;

internal class QuestionImageConfiguration : IEntityTypeConfiguration<QuestionImage>
{
    public void Configure(EntityTypeBuilder<QuestionImage> builder)
    {
        builder.ToTable("questions_images");

        builder.Property(img => img.Id).HasColumnName("image_id");
        builder.Property(img => img.Path).HasColumnName("path");
        builder.Property(img => img.QuestionId).HasColumnName("question_id");
    }
}
