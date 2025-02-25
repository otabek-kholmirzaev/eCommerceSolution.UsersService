using eCommerce.Core.DTO;
using FluentValidation;

namespace eCommerce.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(temp => temp.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address format");

        RuleFor(temp => temp.Password)
            .NotEmpty().WithMessage("Password is required");

        RuleFor(temp => temp.PersonName)
            .NotEmpty().WithMessage("PersonName is required")
            .Length(1, 50).WithMessage("Person Name should be 1 to 50 characters long");

        RuleFor(temp => temp.Gender)
            .IsInEnum().WithMessage("Invalid gender option");
    }
}
