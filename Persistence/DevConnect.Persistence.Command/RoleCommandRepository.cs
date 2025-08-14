using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;

namespace DevConnect.Persistence.Command;

public class RoleCommandRepository(DevConnectDbContext psqlContext) : IRoleCommandRepository
{
    public async Task AddRoleAsync(Role role, CancellationToken cancellationToken)
    {
        await psqlContext.Roles.AddAsync(role, cancellationToken);
        await psqlContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRoleAsync(Role role, CancellationToken cancellationToken)
    {
        psqlContext.Roles.Update(role);
        await psqlContext.SaveChangesAsync(cancellationToken);
    }
}