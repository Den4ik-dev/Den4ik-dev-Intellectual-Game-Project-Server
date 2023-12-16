using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain.DTOs;
using server.Domain.Models;
using server.Web.ResponseModels;

namespace server.Web.Controllers;
[ApiController, Route("/api/questions")]
public class QuestionsController : ControllerBase
{
  private IQuestionsService _questionsService;
  private ICategoriesQuestionsService _categoriesQuestionsService;

  public QuestionsController(IQuestionsService questionsService, 
    ICategoriesQuestionsService categoriesQuestionsService)
  {
    _questionsService = questionsService;
    _categoriesQuestionsService = categoriesQuestionsService;
  }

  [HttpPost, Authorize(Roles = RoleTypes.ADMIN)]
  public async Task<IActionResult> AddQuestion(
    [FromBody] AddedQuestionDto addedQuestion,
    [FromServices] IValidator<AddedQuestionDto> addedQuestionValidator)
  {
    ValidationResult addedQuestionValidatorResult =
      addedQuestionValidator.Validate(addedQuestion);

    if (!addedQuestionValidatorResult.IsValid)
      return BadRequest(new Response(addedQuestionValidatorResult.Errors.First().ErrorMessage));

    if(await _categoriesQuestionsService.GetCategoryQuestionAsync(addedQuestion.CategoryQuestionId) == null)
      return BadRequest(new Response("Категория с данным идентификатором не найдена"));

    if (await _questionsService.GetQuestionAsync(addedQuestion.Content) != null)
      return BadRequest(new Response("Данный вопрос уже существует"));

    Question question = await _questionsService.AddQuestionAsync(addedQuestion);

    return Ok(new QuestionDto()
    {
      Id = question.Id,
      Content = question.Content,
      CategoryQuestionId = question.CategoryQuestionId
    });
  }

  [HttpPut("{id:int}"), Authorize(Roles = RoleTypes.ADMIN)]
  public async Task<IActionResult> ChangeQuestion(int id,
    [FromBody] ChangedQuestionDto changedQuestion,
    [FromServices] IValidator<ChangedQuestionDto> changedQuestionValidator)
  {
    ValidationResult changedQuestionValidatorResult = 
      changedQuestionValidator.Validate(changedQuestion);

    if (!changedQuestionValidatorResult.IsValid)
      return BadRequest(new Response(changedQuestionValidatorResult.Errors.First().ErrorMessage));

    if (await _categoriesQuestionsService.GetCategoryQuestionAsync(changedQuestion.CategoryQuestionId) == null)
      return BadRequest(new Response("Категория с данным идентификатором не найдена"));

    Question? question = await _questionsService.GetQuestionAsync(id);

    if (question == null)
      return BadRequest(new Response("Вопрос с данным идентификатором не найден"));

    await _questionsService.ChangeQuestionAsync(question, changedQuestion);

    return Ok(new QuestionDto()
    {
      Id = question.Id,
      Content = question.Content,
      CategoryQuestionId = question.CategoryQuestionId,
      ImageId = question?.Image?.Id ?? 0,
      AnswersIds = question?.Answers?.Select(ans => ans.Id).ToArray()
    });
  }

  [HttpDelete("{id:int}"), Authorize(Roles = RoleTypes.ADMIN)]
  public async Task<IActionResult> RemoveQuestion(int id)
  {
    Question? removedQuestion = await _questionsService.GetQuestionAsync(id);

    if (removedQuestion == null)
      return BadRequest(new Response("Вопрос с данным идентификатором не найден"));

    await _questionsService.RemoveQuestionAsync(removedQuestion);

    return Ok();
  }

  [HttpGet, Authorize]
  public async Task<IEnumerable<QuestionDto>> GetRangeOfQuestions(
    [FromQuery] int limit, [FromQuery] int page)
  {
    int countOfQuestions = await _questionsService.CountOfQuestionsAsync();

    Response.Headers.Add(
      "x-total-count",
      countOfQuestions.ToString());

    return _questionsService.GetRangeOfQuestions(limit, page)
      .Select(q => new QuestionDto()
      {
        Id = q.Id,
        Content = q.Content,
        ImageId = q?.Image?.Id ?? 0,
        CategoryQuestionId = q.CategoryQuestionId,
        AnswersIds = q?.Answers?.Select(ans => ans.Id).ToArray()
      });
  }

  [HttpGet("all"), Authorize]
  public IEnumerable<QuestionDto> GetAllQuestions() =>
    _questionsService.GetAllQuestions()
      .Include(q => q.Image)
      .Include(q => q.Answers)
      .AsEnumerable()
      .Select(q => new QuestionDto()
      {
        Id = q.Id,
        Content = q.Content,
        ImageId = q?.Image?.Id ?? 0,
        CategoryQuestionId = q.CategoryQuestionId,
        AnswersIds = q?.Answers?.Select(ans => ans.Id).ToArray()
      });
}