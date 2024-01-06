using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.DTOs;
using server.Domain.Models;
using server.Web.ResponseModels;

namespace server.Web.Controllers;

[ApiController, Route("/api/users/questions")]
public class UsersQuestionsController : ControllerBase
{
    private IUsersQuestionsService _usersQuestionsService;
    private ICategoriesQuestionsService _categoriesQuestionsService;

    public UsersQuestionsController(
        IUsersQuestionsService usersQuestionsService,
        ICategoriesQuestionsService categoriesQuestionsService
    )
    {
        _usersQuestionsService = usersQuestionsService;
        _categoriesQuestionsService = categoriesQuestionsService;
    }

    [HttpPost("ask"), Authorize]
    public async Task<AskUserQuestionDto> GetNewUserQuestion()
    {
        int userId = int.Parse(User.Identity.Name);

        return await _usersQuestionsService.GetNewAskUserQuestion(userId);
    }

    [HttpPost("ask/{categoryQuestionId:int}"), Authorize]
    public async Task<IActionResult> GetNewUserQuestionByCategory(int categoryQuestionId)
    {
        int userId = int.Parse(User.Identity.Name);

        if (await _categoriesQuestionsService.GetCategoryQuestionAsync(categoryQuestionId) == null)
            return BadRequest(new Response("Категория вопроса не найдена"));

        return Ok(
            await _usersQuestionsService.GetNewAskUserQuestionByCategory(userId, categoryQuestionId)
        );
    }

    [HttpPost("answer"), Authorize]
    public async Task<IActionResult> AnswerTheQuestion(
        [FromBody] AnswerUserQuestionDto answerUserQuestion
    )
    {
        int userId = int.Parse(User.Identity.Name);

        UserQuestion? userQuestion = await _usersQuestionsService.GetUserQuestionAsync(
            uq => uq.Id == answerUserQuestion.UserQuestionId && uq.UserId == userId
        );

        if (userQuestion == null)
            return BadRequest(new Response("Вопрос, на который отвечал пользователь, не найден"));

        if (userQuestion.AnswerNumber != 0)
            return BadRequest(new Response("Пользователь уже отвечал на данный вопрос"));

        if (userQuestion.UserQuestionExpiryTime <= DateTime.UtcNow)
            return BadRequest(new Response("Время ответита на вопрос истекло"));

        if (
            answerUserQuestion.AnswerNumber <= 0
            || answerUserQuestion.AnswerNumber > userQuestion.Question?.Answers?.Count()
        )
            return BadRequest(new Response("Нет такого варианта ответа"));

        if (
            await _usersQuestionsService.CheckingAnswerUserQuestionAsync(
                answerUserQuestion,
                userQuestion
            )
        )
            return Ok(new { IsTrue = true, TrueAnswerNumber = answerUserQuestion.AnswerNumber });

        int trueAnswerNumber = _usersQuestionsService.GetTrueAnswerNumberUserQuestion(userQuestion);

        return Ok(new { IsTrue = false, TrueAnswerNumber = trueAnswerNumber });
    }

    [HttpPost("correct/answer/{idUserQuestion:int}"), Authorize]
    public async Task<IActionResult> GetCorrectAnswer(int idUserQuestion)
    {
        int userId = int.Parse(User.Identity.Name);

        UserQuestion? userQuestion = await _usersQuestionsService.GetUserQuestionAsync(
            uq => uq.Id == idUserQuestion && uq.UserId == userId
        );

        if (userQuestion == null)
            return BadRequest(new Response("Вопрос, на который отвечал пользователь, не найден"));

        if (userQuestion.AnswerNumber != 0)
            return BadRequest(new Response("Пользователь уже отвечал на данный вопрос"));

        await _usersQuestionsService.SetUserQuestionExpiryTimeAsync(userQuestion);

        int trueAnswerNumber = _usersQuestionsService.GetTrueAnswerNumberUserQuestion(userQuestion);

        return Ok(new { IsTrue = false, TrueAnswerNumber = trueAnswerNumber });
    }

    [HttpGet("all"), Authorize]
    public IEnumerable<UserQuestionDto> GetAllUserQuestions()
    {
        int userId = int.Parse(User.Identity.Name);

        return _usersQuestionsService
            .GetAllUserQuestions(userId)
            .Select(
                uq =>
                    new UserQuestionDto()
                    {
                        Id = uq.Id,
                        UserId = uq.UserId,
                        CategoryQuestionId = uq.Question.CategoryQuestionId,
                        QuestionId = uq.QuestionId,
                        QuestionContent = uq.Question.Content,
                        Complete = uq.Complete,
                        AnswerNumber = uq.AnswerNumber
                    }
            );
    }

    [HttpGet, Authorize]
    public async Task<IEnumerable<UserQuestionDto>> GetRangeOfUserQuestions(
        [FromQuery] int limit,
        [FromQuery] int page
    )
    {
        int userId = int.Parse(User.Identity.Name);

        int countOfUserQuestions = await _usersQuestionsService.CountOfUserQuestions(userId);

        Response.Headers.Add("x-total-count", countOfUserQuestions.ToString());

        return _usersQuestionsService
            .GetRangeOfUserQuestion(userId, limit, page)
            .Select(
                uq =>
                    new UserQuestionDto()
                    {
                        Id = uq.Id,
                        UserId = uq.UserId,
                        CategoryQuestionId = uq.Question.CategoryQuestionId,
                        QuestionId = uq.QuestionId,
                        QuestionContent = uq.Question.Content,
                        Complete = uq.Complete,
                        AnswerNumber = uq.AnswerNumber
                    }
            );
    }

    [HttpGet("{userQuestionId:int}"), Authorize]
    public async Task<IActionResult> GetDetailsUserQuestionById(int userQuestionId)
    {
        int userId = int.Parse(User.Identity.Name);

        UserQuestion? userQuestion = await _usersQuestionsService.GetUserQuestionAsync(
            uq => uq.Id == userQuestionId && uq.UserId == userId
        );

        if (userQuestion == null)
            return BadRequest(new Response("Ваш вопрос не найден"));

        return Ok(_usersQuestionsService.GetUserQuestionDetails(userQuestion));
    }
}
