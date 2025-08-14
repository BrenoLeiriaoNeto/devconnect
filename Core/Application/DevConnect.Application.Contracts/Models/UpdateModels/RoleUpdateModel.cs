using DevConnect.Domain.Enums;

namespace DevConnect.Application.Contracts.Models.UpdateModels;

public class RoleUpdateModel
{
    public Guid RoleId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public RoleType Type { get; set; }
    public List<string> Permissions { get; set; } = new();
}