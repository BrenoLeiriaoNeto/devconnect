namespace DevConnect.Application.Contracts.Interfaces.Common;

public interface IMessageHandler
{
    Task HandleMessageAsync(string message);   
}