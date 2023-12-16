using FluentValidation;
using server.Domain.DTOs;

namespace server.Infrastructure.Validation;
public class AddedQuestionDtoValidator : AbstractValidator<AddedQuestionDto>
{
  public AddedQuestionDtoValidator()
  {
    RuleFor(q => q.Content)
      .NotNull().WithMessage("Вопрос не может иметь значение null")
      .NotEmpty().WithMessage("Вы не указали вопрос");

    RuleFor(q => q.CategoryQuestionId)
      .Must(cqi => cqi > 0).WithMessage("Категории с данным идентификатором не может существовать");
  }
}