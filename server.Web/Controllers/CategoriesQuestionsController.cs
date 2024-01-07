using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.DTOs;
using server.Domain.Models;
using server.Web.ResponseModels;

namespace server.Web.Controllers;

[ApiController, Route("/api/questions/categories")]
public class CategoriesQuestionsController : ControllerBase
{
    private ICategoriesQuestionsService _categoriesQuestionsService;

    public CategoriesQuestionsController(ICategoriesQuestionsService categoriesQuestionsService)
    {
        _categoriesQuestionsService = categoriesQuestionsService;
    }

    [HttpPost, Authorize(Roles = RoleTypes.ADMIN)]
    public async Task<IActionResult> AddCategoryQuestion(
        [FromBody] AddedCategoryQuestionDto addedCategoryQuestion,
        [FromServices] IValidator<AddedCategoryQuestionDto> addedCategoryQuestionValidator
    )
    {
        ValidationResult addedCategoryQuestionValidatorResult =
            addedCategoryQuestionValidator.Validate(addedCategoryQuestion);

        if (!addedCategoryQuestionValidatorResult.IsValid)
            return BadRequest(
                new Response(addedCategoryQuestionValidatorResult.Errors.First().ErrorMessage)
            );

        if (
            await _categoriesQuestionsService.GetCategoryQuestionAsync(addedCategoryQuestion.Title)
            != null
        )
            return BadRequest(new Response("Категория с данным названием уже существует"));

        CategoryQuestion categoryQuestion =
            await _categoriesQuestionsService.AddCategoryQuestionAsync(addedCategoryQuestion);

        return Ok(
            new CategoryQuestionDto() { Id = categoryQuestion.Id, Title = categoryQuestion.Title }
        );
    }

    [HttpDelete("{id:int}"), Authorize(Roles = RoleTypes.ADMIN)]
    public async Task<IActionResult> RemoveCategoryQuestion(int id)
    {
        CategoryQuestion? removedCategoryQuestion =
            await _categoriesQuestionsService.GetCategoryQuestionAsync(id);

        if (removedCategoryQuestion == null)
            return BadRequest(new Response("Не удалось удалить категорию, т.к она не найдена!"));

        await _categoriesQuestionsService.RemoveCategoryQuestionAsync(removedCategoryQuestion);

        return Ok();
    }

    [HttpPut("{id:int}"), Authorize(Roles = RoleTypes.ADMIN)]
    public async Task<IActionResult> ChangeCategoryQuestion(
        int id,
        [FromBody] ChangedCategoryQuestionDto changedCategoryQuestion,
        [FromServices] IValidator<ChangedCategoryQuestionDto> changedCategoryQuestionValidator
    )
    {
        ValidationResult changedCategoryQuestionValidatorResult =
            changedCategoryQuestionValidator.Validate(changedCategoryQuestion);

        if (!changedCategoryQuestionValidatorResult.IsValid)
            return BadRequest(
                new Response(changedCategoryQuestionValidatorResult.Errors.First().ErrorMessage)
            );

        CategoryQuestion? categoryQuestion =
            await _categoriesQuestionsService.GetCategoryQuestionAsync(id);

        if (categoryQuestion == null)
            return BadRequest(new Response("Не удалось изменить категорию, т.к она не найдена!"));

        await _categoriesQuestionsService.ChangeCategoryQuestionAsync(
            categoryQuestion,
            changedCategoryQuestion
        );

        return Ok(
            new CategoryQuestionDto() { Id = categoryQuestion.Id, Title = categoryQuestion.Title }
        );
    }

    [HttpGet, Authorize]
    public async Task<IEnumerable<CategoryQuestionDto>> GetRangeOfCategoriesQuestions(
        [FromQuery] int limit,
        [FromQuery] int page
    )
    {
        int countOfCategoriesQuestions =
            await _categoriesQuestionsService.CountOfCategoriesQuestionsAsync();

        Response.Headers.Add("x-total-count", countOfCategoriesQuestions.ToString());

        return _categoriesQuestionsService
            .GetRangeOfCategoriesQuestions(limit, page)
            .Select(
                cq =>
                    new CategoryQuestionDto()
                    {
                        Id = cq.Id,
                        Title = cq.Title,
                        ImagePath = cq.Image.Path
                    }
            );
    }

    [HttpGet("all"), Authorize]
    public IEnumerable<CategoryQuestionDto> GetAllCategoriesQuestions() =>
        _categoriesQuestionsService
            .GetAllCategoriesQuestions()
            .Select(
                cq =>
                    new CategoryQuestionDto()
                    {
                        Id = cq.Id,
                        Title = cq.Title,
                        ImagePath = cq.Image.Path
                    }
            );
}
