using MediatR;

namespace DevConnect.Application.Services.Infra.Commands;

public record EmailVerificationCommand(string VerificationToken) : IRequest<bool>;