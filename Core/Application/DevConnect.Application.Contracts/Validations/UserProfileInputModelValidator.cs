using DevConnect.Application.Contracts.Models.InputModels;
using FluentValidation;

namespace DevConnect.Application.Contracts.Validations;

public class UserProfileInputModelValidator : AbstractValidator<UserProfileInputModel>
{
    public UserProfileInputModelValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Bio)
            .MaximumLength(1000).WithMessage("Bio can't be longer than 1000 characters.");

        RuleFor(x => x.Location)
            .MaximumLength(255);
        
        RuleFor(x => x.ProfilePictureUrl)
            .MaximumLength(500)
            .Must(BeValidUrl).WithMessage("Profile picture URL is invalid.");
    }

    private static bool BeValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}