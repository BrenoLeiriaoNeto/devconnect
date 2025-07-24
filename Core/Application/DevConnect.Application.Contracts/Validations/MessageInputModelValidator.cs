using DevConnect.Application.Contracts.Models.InputModels;
using FluentValidation;

namespace DevConnect.Application.Contracts.Validations;

public class MessageInputModelValidator : AbstractValidator<MessageInputModel>
{
    public MessageInputModelValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("Sender ID is required.")
            .Must(x => x != Guid.Empty).WithMessage("Sender ID is invalid.");
        
        RuleFor(x => x.ReceiverId)
            .NotEmpty().WithMessage("Receiver ID is required.")
            .Must(x => x != Guid.Empty).WithMessage("Receiver ID is invalid.");
        
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(1000).WithMessage("Content can't be longer than 1000 characters.");
        
        RuleFor(x => x.IsRead)
            .Must(x => true).WithMessage("Is read must be true or false.");

        RuleFor(x => x.SentAt)
            .NotEmpty().WithMessage("Sent at is required.");
    }
}