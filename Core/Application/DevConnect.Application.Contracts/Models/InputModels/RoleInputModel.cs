using DevConnect.Domain.Enums;

namespace DevConnect.Application.Contracts.Models.InputModels;

public class RoleInputModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public RoleType Type { get; set; } = RoleType.User;
    public List<string> Permissions { get; set; } = new();
}