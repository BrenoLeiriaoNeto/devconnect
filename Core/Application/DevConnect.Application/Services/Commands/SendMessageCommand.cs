using DevConnect.Application.Contracts.Models.InputModels;
using MediatR;

namespace DevConnect.Application.Services.Commands;

public class SendMessageCommand(MessageInputModel input) : IRequest
{
    public MessageInputModel Input { get; } = input;
}