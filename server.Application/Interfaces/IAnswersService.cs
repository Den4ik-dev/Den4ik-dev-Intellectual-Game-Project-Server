using server.Domain.DTOs;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface IAnswersService
{
  public Task<Answer> AddAnswerAsync(AddedAnswerDto addedAnswer);
  public Task<Answer?> GetAnswerAsync(int id);
  public Task<Answer?> GetAnswerAsync(Expression<Func<Answer, bool>> predicate);
  public Task ChangeAnswerAsync(Answer initialAnswer, ChangedAnswerDto finalAnswer);
  public Task RemoveAnswerAsync(Answer removedAnswer);
  public IEnumerable<Answer> GetAllAnswersByQuestionId(int questionId);
}