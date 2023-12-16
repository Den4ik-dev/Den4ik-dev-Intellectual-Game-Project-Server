﻿using FluentValidation;
using server.Domain.DTOs;

namespace server.Infrastructure.Validation;
public class AddedAnswerDtoValidator : AbstractValidator<AddedAnswerDto>
{
  public AddedAnswerDtoValidator()
  {
    RuleFor(ans => ans.Content)
      .NotNull().WithMessage("Ответ не может иметь значение null")
      .NotEmpty().WithMessage("Вы не указали содержимое ответа");

    RuleFor(ans => ans.QuestionId)
      .Must(qi => qi > 0).WithMessage("Вопрос с данным идентификатором не может существовать");
  }
}