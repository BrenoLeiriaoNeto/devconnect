using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Query;

public interface IRoleQueryRepository
{
    Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken);
    Task<List<Role>> GetDefaultRolesAsync(CancellationToken cancellationToken);
}