using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Models;

namespace server.Domain.Database.EntityTypeConfigurations;

public class CategoryQuestionImageConfiguration : IEntityTypeConfiguration<CategoryQuestionImage>
{
    public void Configure(EntityTypeBuilder<CategoryQuestionImage> builder)
    {
        builder.ToTable("categories_questions_images");

        builder.Property(cqi => cqi.Id).HasColumnName("category_question_image_id");
        builder.Property(cqi => cqi.Path).HasColumnName("path");
        builder.Property(cqi => cqi.CategoryQuestionId).HasColumnName("category_question_id");
    }
}
