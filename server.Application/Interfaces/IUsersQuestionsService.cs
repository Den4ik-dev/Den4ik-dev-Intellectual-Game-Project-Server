﻿using server.Domain.DTOs;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface IUsersQuestionsService
{
  public Task<AskUserQuestionDto> GetNewAskUserQuestion(int userId);
  public Task<UserQuestion?> GetUserQuestionAsync(Expression<Func<UserQuestion, bool>> predicate);
  public Task<bool> CheckingAnswerUserQuestionAsync(
    AnswerUserQuestionDto answerUserQuestion, UserQuestion userQuestion);
  public int GetTrueAnswerNumberUserQuestion(UserQuestion userQuestion);
  public IEnumerable<UserQuestion> GetAllUserQuestions(int userId);
  public IEnumerable<UserQuestion> GetRangeOfUserQuestion(int userId, int limit, int page);
  public Task<int> CountOfUserQuestions(int userId);
  public Task SetUserQuestionExpiryTimeAsync(UserQuestion userQuestion);
}
