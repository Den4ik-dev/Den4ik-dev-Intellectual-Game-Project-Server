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

  public UsersQuestionsController(IUsersQuestionsService usersQuestionsService)
  {
    _usersQuestionsService = usersQuestionsService;
  }

  [HttpGet("ask"), Authorize]
  public async Task<UserQuestionDto> GetNewUserQuestion()
  {
    int userId = int.Parse(User.Identity.Name);

    return await _usersQuestionsService.GetNewUserQuestion(userId);
  }

  [HttpPost("answer"), Authorize]
  public async Task<IActionResult> AnswerTheQuestion(
    [FromBody] AnswerUserQuestionDto answerUserQuestion)
  {
    int userId = int.Parse(User.Identity.Name);

    UserQuestion? userQuestion = await _usersQuestionsService.GetUserQuestionAsync(uq => 
      uq.Id == answerUserQuestion.UserQuestionId && uq.UserId == userId);

    if (userQuestion == null)
      return BadRequest(new Response("Вопрос, на который отвечал пользователь, не найден"));
    
    
  }
}