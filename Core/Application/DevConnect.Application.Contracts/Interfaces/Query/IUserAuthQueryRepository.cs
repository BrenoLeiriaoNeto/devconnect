using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Query;

public interface IUserAuthQueryRepository
{
    Task<UserAuth?> GetByEmailAsync(string email);
    Task<UserAuth?> GetByIdAsync(Guid id);
}