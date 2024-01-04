using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.DTOs;
using server.Domain.Models;
using server.Web.ResponseModels;

namespace server.Web.Controllers;

[ApiController, Route("/api/questions/answers")]
public class AnswersController : ControllerBase
{
    private IAnswersService _answersService;
    private IQuestionsService _questionsService;

    public AnswersController(IAnswersService answersService, IQuestionsService questionsService)
    {
        _answersService = answersService;
        _questionsService = questionsService;
    }

    [HttpPost, Authorize(Roles = RoleTypes.ADMIN)]
    public async Task<IActionResult> AddAnswer(
        [FromBody] AddedAnswerDto addedAnswer,
        [FromServices] IValidator<AddedAnswerDto> addedAnswerValidator
    )
    {
        ValidationResult addedAnswerValidatorResult = addedAnswerValidator.Validate(addedAnswer);

        if (!addedAnswerValidatorResult.IsValid)
            return BadRequest(new Response(addedAnswerValidatorResult.Errors.First().ErrorMessage));

        if (await _questionsService.GetQuestionAsync(addedAnswer.QuestionId) == null)
            return BadRequest(new Response("Вопрос с данным идентификатором не найден"));

        Answer answer = await _answersService.AddAnswerAsync(addedAnswer);

        return Ok(
            new AnswerDto()
            {
                Id = answer.Id,
                Content = answer.Content,
                IsTrue = answer.IsTrue,
                QuestionId = answer.QuestionId
            }
        );
    }

    [HttpPut("{id:int}"), Authorize(Roles = RoleTypes.ADMIN)]
    public async Task<IActionResult> ChangeAnswer(
        int id,
        [FromBody] ChangedAnswerDto changedAnswer,
        [FromServices] IValidator<ChangedAnswerDto> changedAnswerDtoValidator
    )
    {
        ValidationResult changedAnswerDtoValidatorResult = changedAnswerDtoValidator.Validate(
            changedAnswer
        );

        if (!changedAnswerDtoValidatorResult.IsValid)
            return BadRequest(
                new Response(changedAnswerDtoValidatorResult.Errors.First().ErrorMessage)
            );

        if (await _questionsService.GetQuestionAsync(changedAnswer.QuestionId) == null)
            return BadRequest(new Response("Вопрос с данным идентификатором не найден"));

        Answer? initialAnswer = await _answersService.GetAnswerAsync(id);

        if (initialAnswer == null)
            return BadRequest(new Response("Ответ с данным идентификатором не найден"));

        await _answersService.ChangeAnswerAsync(initialAnswer, changedAnswer);

        return Ok(
            new AnswerDto()
            {
                Id = initialAnswer.Id,
                Content = initialAnswer.Content,
                IsTrue = initialAnswer.IsTrue,
                QuestionId = initialAnswer.QuestionId
            }
        );
    }

    [HttpDelete("{id:int}"), Authorize(Roles = RoleTypes.ADMIN)]
    public async Task<IActionResult> RemoveAnswer(int id)
    {
        Answer? removedAnswer = await _answersService.GetAnswerAsync(id);

        if (removedAnswer == null)
            return BadRequest("Ответ с данным идентификатором не найден");

        await _answersService.RemoveAnswerAsync(removedAnswer);

        return Ok();
    }

    [HttpGet("{questionId:int}"), Authorize]
    public IEnumerable<AnswerDto> GetAllAnswers(int questionId) =>
        _answersService
            .GetAllAnswersByQuestionId(questionId)
            .Select(
                ans =>
                    new AnswerDto()
                    {
                        Id = ans.Id,
                        Content = ans.Content,
                        IsTrue = ans.IsTrue,
                        QuestionId = ans.QuestionId
                    }
            );
}
