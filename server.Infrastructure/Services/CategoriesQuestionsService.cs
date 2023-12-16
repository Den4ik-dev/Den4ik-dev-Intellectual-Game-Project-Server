using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain.Database;
using server.Domain.DTOs;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Infrastructure.Services;
public class CategoriesQuestionsService : ICategoriesQuestionsService
{
  private ApplicationContext _db;
  public CategoriesQuestionsService(ApplicationContext db)
  {
    _db = db;
  }

  public async Task<CategoryQuestion?> GetCategoryQuestionAsync(string categoryQuestionTitle) =>
    await _db.CategoryQuestions.FirstOrDefaultAsync(cq => cq.Title == categoryQuestionTitle);

  public async Task<CategoryQuestion?> GetCategoryQuestionAsync(int categoryQuestionId) =>
    await _db.CategoryQuestions.FindAsync(categoryQuestionId);

  public async Task<CategoryQuestion?> GetCategoryQuestionAsync(
    Expression<Func<CategoryQuestion, bool>> predicate) =>
    await _db.CategoryQuestions.FirstOrDefaultAsync(predicate);

  public async Task<CategoryQuestion> AddCategoryQuestionAsync(
    AddedCategoryQuestionDto addedCategoryQuestion)
  {
    CategoryQuestion categoryQuestion = new CategoryQuestion()
    {
      Title = addedCategoryQuestion.Title
    };

    _db.CategoryQuestions.Add(categoryQuestion);
    await _db.SaveChangesAsync();

    return categoryQuestion;
  }

  public async Task RemoveCategoryQuestionAsync(CategoryQuestion removedCategoryQuestion)
  {
    _db.CategoryQuestions.Remove(removedCategoryQuestion);

    await _db.SaveChangesAsync();
  }

  public async Task ChangeCategoryQuestionAsync(
    CategoryQuestion initialCategoryQuestion, 
    ChangedCategoryQuestionDto finalCategoryQuestion)
  {
    initialCategoryQuestion.Title = finalCategoryQuestion.Title;

    await _db.SaveChangesAsync();
  }

  public async Task<int> CountOfCategoriesQuestionsAsync() =>
    await _db.CategoryQuestions.CountAsync();

  public IEnumerable<CategoryQuestion> GetRangeOfCategoriesQuestions(int limit, int page) =>
    _db.CategoryQuestions
      .Skip(limit * page)
      .Take(limit);

  public IEnumerable<CategoryQuestion> GetAllCategoriesQuestions() =>
    _db.CategoryQuestions;
}