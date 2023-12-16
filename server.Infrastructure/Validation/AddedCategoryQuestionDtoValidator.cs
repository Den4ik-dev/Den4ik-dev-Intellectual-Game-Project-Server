using FluentValidation;
using server.Domain.DTOs;

namespace server.Infrastructure.Validation;
public class AddedCategoryQuestionDtoValidator : AbstractValidator<AddedCategoryQuestionDto>
{
  public AddedCategoryQuestionDtoValidator()
  {
    RuleFor(cq => cq.Title)
      .NotNull().WithMessage("Название категории не может принимать значение null")
      .NotEmpty().WithMessage("Вы не указали название категории");
  }
}
