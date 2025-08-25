namespace MyDotNetBase.Domain.Shared.Auditing;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    string? CreatedBy { get; }
    DateTime? UpdatedAt { get; }
    string? UpdatedBy { get; }

    void SetCreated(string userId);
    void SetUpdated(string userId);
}
