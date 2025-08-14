using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Persistence.Query;

public class RoleQueryRepository(DevConnectDbContext psqlContext) : IRoleQueryRepository
{
    public async Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken) => 
        await psqlContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);

    public async Task<List<Role>> GetDefaultRolesAsync(CancellationToken cancellationToken) => 
        await psqlContext.Roles.ToListAsync(cancellationToken);
}