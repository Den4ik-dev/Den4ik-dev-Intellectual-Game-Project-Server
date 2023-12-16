using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Models;

namespace server.Domain.Database.EntityTypeConfigurations;
internal class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
  public void Configure(EntityTypeBuilder<Question> builder)
  {
    builder.ToTable("questions");

    builder.Property(q => q.Id).HasColumnName("question_id");
    builder.Property(q => q.Content).HasColumnName("content");
    builder.Property(q => q.CategoryQuestionId).HasColumnName("category_question_id");
  }
}
