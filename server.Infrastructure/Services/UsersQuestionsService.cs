using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain.Database;
using server.Domain.DTOs;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Infrastructure.Services;
public class UsersQuestionsService : IUsersQuestionsService
{
  private ApplicationContext _db;
  private IQuestionsService _questionsService;
  
  public UsersQuestionsService(ApplicationContext db, 
    IQuestionsService questionsService)
  {
    _db = db;
    _questionsService = questionsService;
  }

  public async Task<UserQuestionDto> GetNewUserQuestion(int userId)
  {
    Question randomQuestion = await GetRandomQuestion();

    UserQuestion newUserQuestion = new UserQuestion()
    {
      UserId = userId,
      QuestionId = randomQuestion.Id,
      UserQuestionExpiryTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(2))
    };

    _db.UserQuestions.Add(newUserQuestion);
    await _db.SaveChangesAsync();

    return new UserQuestionDto
    {
      Id = newUserQuestion.Id,
      UserId = newUserQuestion.UserId,
      CategoryQuestionId = randomQuestion.CategoryQuestionId,
      QuestionContent = randomQuestion.Content,
      Answers = randomQuestion.Answers?.Select(ans => ans.Content).ToArray(),
      ImagePath = randomQuestion.Image?.Path
    };
  }

  public async Task<UserQuestion?> GetUserQuestionAsync(
    Expression<Func<UserQuestion, bool>> predicate) => 
      await _db.UserQuestions.FirstOrDefaultAsync(predicate);

  public async Task<bool> CheckingAnswerUserQuestion(
    AnswerUserQuestionDto answerUserQuestion, UserQuestion userQuestion)
  {
    Answer? answer =
      userQuestion.Question?.Answers?.ToListAsync()[answerUserQuestion.AnswerNumber];

  }

  private async Task<Question> GetRandomQuestion()
  {
    Random random = new Random();
    int countOfQuestions = await _db.Questions.CountAsync();

    int randomQuestionIndex = random.Next(countOfQuestions);
    
    Question randomQuestion = (await _db.Questions.ToListAsync())[randomQuestionIndex];

    return randomQuestion;
  }
}