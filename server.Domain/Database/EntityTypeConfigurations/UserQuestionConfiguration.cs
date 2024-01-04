using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Models;

namespace server.Domain.Database.EntityTypeConfigurations;

internal class UserQuestionConfiguration : IEntityTypeConfiguration<UserQuestion>
{
    public void Configure(EntityTypeBuilder<UserQuestion> builder)
    {
        builder.ToTable("users_questions");

        builder.Property(uq => uq.Id).HasColumnName("user_question_id");
        builder.Property(uq => uq.UserId).HasColumnName("user_id");
        builder.Property(uq => uq.QuestionId).HasColumnName("question_id");
        builder.Property(uq => uq.Complete).HasColumnName("complete");
        builder.Property(uq => uq.AnswerNumber).HasColumnName("answer_number");
        builder
            .Property(uq => uq.UserQuestionExpiryTime)
            .HasColumnName("user_question_expiry_time");
    }
}
