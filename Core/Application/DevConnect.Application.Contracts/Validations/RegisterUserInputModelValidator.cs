using DevConnect.Application.Contracts.Models.InputModels;
using FluentValidation;

namespace DevConnect.Application.Contracts.Validations;

public class RegisterUserInputModelValidator : AbstractValidator<RegisterUserInputModel>
{
    public RegisterUserInputModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username can't be shorter than 3 characters.")
            .MaximumLength(50).WithMessage("Username can't be longer than 50 characters.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is invalid.");
        
        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password can't be shorter than 8 characters.")
            .MaximumLength(20).WithMessage("Password can't be longer than 20 characters.");
    }
}