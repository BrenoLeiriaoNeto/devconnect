using DevConnect.Application.Contracts.Models.InputModels;
using FluentValidation;

namespace DevConnect.Application.Contracts.Validations;

public class LoginUserInputModelValidator : AbstractValidator<LoginUserInputModel>
{
    public LoginUserInputModelValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("Email or username is required.")
            .MaximumLength(50).WithMessage("Email or username can't be longer than 50 characters.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password can't be shorter than 8 characters.")
            .MaximumLength(20).WithMessage("Password can't be longer than 20 characters.");
    }
}