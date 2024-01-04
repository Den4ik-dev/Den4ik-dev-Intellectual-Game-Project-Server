using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Models;

namespace server.Domain.Database.EntityTypeConfigurations;

internal class CategoryQuestionConfiguration : IEntityTypeConfiguration<CategoryQuestion>
{
    public void Configure(EntityTypeBuilder<CategoryQuestion> builder)
    {
        builder.ToTable("categories_questions");

        builder.Property(cq => cq.Id).HasColumnName("category_question_id");
        builder.Property(cq => cq.Title).HasColumnName("title");
    }
}
