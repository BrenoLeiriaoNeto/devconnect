using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Command;

public interface IUserAuthCommandRepository
{
    Task AddAsync(UserAuth userAuth);
    Task<bool> ExistsAsync(string email);
}