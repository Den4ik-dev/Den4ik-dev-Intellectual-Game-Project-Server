using server.Domain.DTOs;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface IQuestionsService
{
  public Task<Question?> GetQuestionAsync(int id);
  public Task<Question?> GetQuestionAsync(string content);
  public Task<Question?> GetQuestionAsync(Expression<Func<Question, bool>> predicate);
  public Task<Question> AddQuestionAsync(AddedQuestionDto addedQuestion);
  public Task ChangeQuestionAsync(Question initialQuestion, ChangedQuestionDto finalQuestion);
  public Task RemoveQuestionAsync(Question removedQuestion);
  public Task<int> CountOfQuestionsAsync();
  public IEnumerable<Question> GetRangeOfQuestions(int limit, int page);
  public IQueryable<Question> GetAllQuestions();
}
