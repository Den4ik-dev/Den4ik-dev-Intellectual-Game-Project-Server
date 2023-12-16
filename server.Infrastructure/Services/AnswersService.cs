using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain.Database;
using server.Domain.DTOs;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Infrastructure.Services;
public class AnswersService : IAnswersService
{
  private ApplicationContext _db;

  public AnswersService(ApplicationContext db)
  {
    _db = db;
  }

  public async Task<Answer> AddAnswerAsync(AddedAnswerDto addedAnswer)
  {
    Answer answer = new Answer()
    {
      Content = addedAnswer.Content,
      IsTrue = addedAnswer.IsTrue,
      QuestionId = addedAnswer.QuestionId
    };

    _db.Answers.Add(answer);
    await _db.SaveChangesAsync();

    return answer;
  }

  public async Task<Answer?> GetAnswerAsync(int id) =>
    await _db.Answers.FindAsync(id);

  public async Task<Answer?> GetAnswerAsync(Expression<Func<Answer, bool>> predicate) =>
    await _db.Answers.FirstOrDefaultAsync(predicate);

  public async Task ChangeAnswerAsync(Answer initialAnswer, ChangedAnswerDto finalAnswer)
  {
    initialAnswer.Content = finalAnswer.Content;
    initialAnswer.IsTrue = finalAnswer.IsTrue;
    initialAnswer.QuestionId = finalAnswer.QuestionId;

    await _db.SaveChangesAsync();
  }

  public async Task RemoveAnswerAsync(Answer removedAnswer)
  {
    _db.Answers.Remove(removedAnswer);

    await _db.SaveChangesAsync();
  }

  public IEnumerable<Answer> GetAllAnswersByQuestionId(int questionId) => 
    _db.Answers.Where(ans => ans.QuestionId == questionId);
}