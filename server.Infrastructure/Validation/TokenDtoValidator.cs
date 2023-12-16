using FluentValidation;
using server.Domain.DTOs;

namespace server.Infrastructure.Validation;
public class TokenDtoValidator : AbstractValidator<TokenDto>
{
  public TokenDtoValidator()
  {
    RuleFor(t => t.AccessToken)
      .NotNull().WithMessage("Access token не может иметь значение null")
      .NotEmpty().WithMessage("Access token не может быть пустым");

    RuleFor(t => t.RefreshToken)
      .NotNull().WithMessage("Refresh token не может иметь значение null")
      .NotEmpty().WithMessage("Refresh token не может быть пустым");
  }
}
