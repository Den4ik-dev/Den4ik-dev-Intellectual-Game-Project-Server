using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Models;

namespace server.Domain.Database.EntityTypeConfigurations;

internal class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("answers");

        builder.Property(ans => ans.Id).HasColumnName("answer_id");
        builder.Property(ans => ans.Content).HasColumnName("content");
        builder.Property(ans => ans.IsTrue).HasColumnName("is_true");
        builder.Property(ans => ans.QuestionId).HasColumnName("question_id");
    }
}
