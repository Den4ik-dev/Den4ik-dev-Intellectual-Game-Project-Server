using server.Domain.DTOs;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface IUsersQuestionsService
{
  public Task<UserQuestionDto> GetNewUserQuestion(int userId);
  public Task<UserQuestion?> GetUserQuestionAsync(Expression<Func<UserQuestion, bool>> predicate);
}
