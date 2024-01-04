using FluentValidation;
using server.Domain.DTOs;

namespace server.Infrastructure.Validation;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(u => u.Login)
            .NotNull()
            .WithMessage("Логин не может иметь значение null")
            .NotEmpty()
            .WithMessage("Вы не указали логин");

        RuleFor(u => u.Password)
            .NotNull()
            .WithMessage("Пароль не может иметь значение null")
            .NotEmpty()
            .WithMessage("Вы не указали пароль");
    }
}
