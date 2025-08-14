using MediatR;

namespace DevConnect.Application.Services.Auth.Commands;

public record ResendVerificationEmailCommand(Guid UserId) : IRequest;