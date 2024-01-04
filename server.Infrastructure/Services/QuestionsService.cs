using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain.Database;
using server.Domain.DTOs;
using server.Domain.Models;

namespace server.Infrastructure.Services;

public class QuestionsService : IQuestionsService
{
    private ApplicationContext _db;

    public QuestionsService(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<Question?> GetQuestionAsync(int id) => await _db.Questions.FindAsync(id);

    public async Task<Question?> GetQuestionAsync(string content) =>
        await _db.Questions.FirstOrDefaultAsync(q => q.Content == content);

    public async Task<Question?> GetQuestionAsync(Expression<Func<Question, bool>> predicate) =>
        await _db.Questions.FirstOrDefaultAsync(predicate);

    public async Task<Question> AddQuestionAsync(AddedQuestionDto addedQuestion)
    {
        Question question = new Question()
        {
            Content = addedQuestion.Content,
            CategoryQuestionId = addedQuestion.CategoryQuestionId
        };
        _db.Questions.Add(question);
        await _db.SaveChangesAsync();

        return question;
    }

    public async Task ChangeQuestionAsync(
        Question initialQuestion,
        ChangedQuestionDto finalQuestion
    )
    {
        initialQuestion.Content = finalQuestion.Content;
        initialQuestion.CategoryQuestionId = finalQuestion.CategoryQuestionId;

        await _db.SaveChangesAsync();
    }

    public async Task RemoveQuestionAsync(Question removedQuestion)
    {
        _db.Questions.Remove(removedQuestion);

        await _db.SaveChangesAsync();
    }

    public async Task<int> CountOfQuestionsAsync() => await _db.Questions.CountAsync();

    public IEnumerable<Question> GetRangeOfQuestions(int limit, int page) =>
        _db.Questions.Skip(limit * page).Take(limit);

    public IQueryable<Question> GetAllQuestions() => _db.Questions;
}
