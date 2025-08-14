using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Command;

public interface IRoleCommandRepository
{
    Task AddRoleAsync(Role role, CancellationToken cancellationToken);
    Task UpdateRoleAsync(Role role, CancellationToken cancellationToken);
}