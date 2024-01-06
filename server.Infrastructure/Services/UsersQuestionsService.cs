using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain.Database;
using server.Domain.DTOs;
using server.Domain.Models;

namespace server.Infrastructure.Services;

public class UsersQuestionsService : IUsersQuestionsService
{
    private ApplicationContext _db;
    private IQuestionsService _questionsService;

    public UsersQuestionsService(ApplicationContext db, IQuestionsService questionsService)
    {
        _db = db;
        _questionsService = questionsService;
    }

    public async Task<AskUserQuestionDto> GetNewAskUserQuestion(int userId)
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

        return new AskUserQuestionDto
        {
            Id = newUserQuestion.Id,
            UserId = newUserQuestion.UserId,
            CategoryQuestionId = randomQuestion.CategoryQuestionId,
            QuestionContent = randomQuestion.Content,
            Answers = randomQuestion.Answers?.Select(ans => ans.Content).ToArray(),
            ImagePath = randomQuestion.Image?.Path
        };
    }

    public async Task<AskUserQuestionDto> GetNewAskUserQuestionByCategory(
        int userId,
        int categoryQuestionId
    )
    {
        Question randomQuestion = await GetRandomQuestionByCategory(categoryQuestionId);

        UserQuestion newUserQuestion = new UserQuestion()
        {
            UserId = userId,
            QuestionId = randomQuestion.Id,
            UserQuestionExpiryTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(2))
        };

        _db.UserQuestions.Add(newUserQuestion);
        await _db.SaveChangesAsync();

        return new AskUserQuestionDto
        {
            Id = newUserQuestion.Id,
            UserId = newUserQuestion.UserId,
            CategoryQuestionId = randomQuestion.CategoryQuestionId,
            QuestionContent = randomQuestion.Content,
            Answers = randomQuestion.Answers?.Select(ans => ans.Content).ToArray(),
            ImagePath = randomQuestion.Image?.Path
        };
    }

    public async Task SetUserQuestionExpiryTimeAsync(UserQuestion userQuestion)
    {
        userQuestion.UserQuestionExpiryTime = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task<UserQuestion?> GetUserQuestionAsync(
        Expression<Func<UserQuestion, bool>> predicate
    ) => await _db.UserQuestions.FirstOrDefaultAsync(predicate);

    public async Task<bool> CheckingAnswerUserQuestionAsync(
        AnswerUserQuestionDto answerUserQuestion,
        UserQuestion userQuestion
    )
    {
        List<Answer>? questionAnswers = userQuestion.Question?.Answers?.ToList();

        Answer? userAnswer = questionAnswers?[answerUserQuestion.AnswerNumber - 1];

        userQuestion.AnswerNumber = answerUserQuestion.AnswerNumber;
        userQuestion.Complete = userAnswer.IsTrue;
        await _db.SaveChangesAsync();

        return userAnswer.IsTrue;
    }

    public IEnumerable<UserQuestion> GetAllUserQuestions(int userId) =>
        _db.UserQuestions.Where(uq => uq.UserId == userId);

    public IEnumerable<UserQuestion> GetRangeOfUserQuestion(int userId, int limit, int page) =>
        _db.UserQuestions.Where(uq => uq.UserId == userId)
            .OrderByDescending(uq => uq.Id)
            .Skip(limit * page)
            .Take(limit);

    public async Task<int> CountOfUserQuestions(int userId) =>
        await _db.UserQuestions.CountAsync(uq => uq.UserId == userId);

    public int GetTrueAnswerNumberUserQuestion(UserQuestion userQuestion)
    {
        List<Answer>? questionAnswers = userQuestion.Question?.Answers?.ToList();

        Answer? trueAnswer = questionAnswers?.FirstOrDefault(ans => ans.IsTrue);

        return questionAnswers.IndexOf(trueAnswer) + 1;
    }

    public UserQuestionDetailsDto GetUserQuestionDetails(UserQuestion userQuestion)
    {
        return new UserQuestionDetailsDto()
        {
            ImagePath = userQuestion.Question?.Image?.Path!,
            QuestionContent = userQuestion.Question?.Content!,
            Answers = userQuestion.Question?.Answers.Select(ans => ans.Content).ToArray(),
            AnswerNumber = userQuestion.AnswerNumber,
            TrueAnswerNumber =
                Array.IndexOf(
                    userQuestion.Question?.Answers.ToArray(),
                    userQuestion.Question.Answers.First(ans => ans.IsTrue)
                ) + 1
        };
    }

    private async Task<Question> GetRandomQuestion()
    {
        Random random = new Random();
        int countOfQuestions = await _db.Questions.CountAsync();

        int randomQuestionIndex = random.Next(countOfQuestions);

        Question randomQuestion = (await _db.Questions.ToListAsync())[randomQuestionIndex];

        return randomQuestion;
    }

    private async Task<Question> GetRandomQuestionByCategory(int categoryId)
    {
        Random random = new Random();

        List<Question> rangeOfQuestion = await _db.Questions.Where(
            q => q.CategoryQuestionId == categoryId
        )
            .ToListAsync();

        int countOfQuestions = rangeOfQuestion.Count;

        int randomQuestionIndex = random.Next(countOfQuestions);

        Question randomQuestion = rangeOfQuestion[randomQuestionIndex];

        return randomQuestion;
    }
}
