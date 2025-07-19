namespace DevConnect.Domain;

public interface IBusinessObject
{
    Guid Id { get; set; }

    DateTime CreatedAt { get; set; }

    string CreatedBy { get; set; }

    DateTime? ModifiedAt { get; set; }

    string? ModifiedBy { get; set; }
    
    DateTime? DeletedAt { get; set; }
    
    string? DeletedBy { get; set; }
}