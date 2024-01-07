using Microsoft.EntityFrameworkCore;
using server.Domain.Database.EntityTypeConfigurations;
using server.Domain.Models;

namespace server.Domain.Database;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<CategoryQuestion> CategoryQuestions { get; set; }
    public DbSet<CategoryQuestionImage> CategoriesQuestionsImages { get; set; }
    public DbSet<QuestionImage> QuestionImages { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserQuestion> UserQuestions { get; set; }
    public DbSet<Role> Roles { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserQuestionConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionImageConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryQuestionConfiguration());
        modelBuilder.ApplyConfiguration(new AnswerConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryQuestionImageConfiguration());
    }
}
