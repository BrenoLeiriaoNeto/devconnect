using MediatR;

namespace DevConnect.Application.Services.Auth.Events;

public record UserRegisteredEvent(Guid UserId, string Email) : INotification;