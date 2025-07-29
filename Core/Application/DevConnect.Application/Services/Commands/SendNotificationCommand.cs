using DevConnect.Application.Contracts.Models.InputModels;
using MediatR;

namespace DevConnect.Application.Services.Commands;

public class SendNotificationCommand(NotificationInputModel input) : IRequest
{
    public NotificationInputModel Input { get; }
}