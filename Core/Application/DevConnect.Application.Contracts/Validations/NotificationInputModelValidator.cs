using DevConnect.Application.Contracts.Models.InputModels;
using FluentValidation;

namespace DevConnect.Application.Contracts.Validations;

public class NotificationInputModelValidator : AbstractValidator<NotificationInputModel>
{
    public NotificationInputModelValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(x => x != Guid.Empty).WithMessage("User ID is invalid.");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title can't be longer than 100 characters.");
        
        RuleFor(x => x.Message)
            .MaximumLength(1000).WithMessage("Message can't be longer than 1000 characters.");
        
        RuleFor(x => x.IsRead)
            .Must(x => true).WithMessage("Is read must be true or false.");
    }
}