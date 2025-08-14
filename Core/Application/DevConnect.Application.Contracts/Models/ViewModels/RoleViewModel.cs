using DevConnect.Domain.Enums;

namespace DevConnect.Application.Contracts.Models.ViewModels;

public class RoleViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public RoleType Type { get; set; }
    public List<string> Permissions { get; set; }
}