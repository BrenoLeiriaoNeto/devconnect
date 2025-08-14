using DevConnect.Domain.Enums;

namespace DevConnect.Domain.Models;

public class Role : BusinessObject
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public RoleType Type { get; set; } = RoleType.User;

    public List<string> Permissions { get; set; } = new();
    
    public ICollection<UserAuth> Users { get; set; } = new List<UserAuth>();
}