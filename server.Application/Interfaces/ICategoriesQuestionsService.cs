using server.Domain.DTOs;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface ICategoriesQuestionsService
{
  public Task<CategoryQuestion?> GetCategoryQuestionAsync(string categoryQuestionTitle);
  public Task<CategoryQuestion?> GetCategoryQuestionAsync(int categoryQuestionId);
  public Task<CategoryQuestion?> GetCategoryQuestionAsync(
    Expression<Func<CategoryQuestion, bool>> predicate);
  public Task<CategoryQuestion> AddCategoryQuestionAsync(AddedCategoryQuestionDto addedCategoryQuestion);
  public Task RemoveCategoryQuestionAsync(CategoryQuestion removedCategoryQuestion);
  public Task ChangeCategoryQuestionAsync(
    CategoryQuestion initialCategoryQuestion,
    ChangedCategoryQuestionDto finalCategoryQuestion);
  public Task<int> CountOfCategoriesQuestionsAsync();
  public IEnumerable<CategoryQuestion> GetRangeOfCategoriesQuestions(int limit, int page);
  public IEnumerable<CategoryQuestion> GetAllCategoriesQuestions();
}