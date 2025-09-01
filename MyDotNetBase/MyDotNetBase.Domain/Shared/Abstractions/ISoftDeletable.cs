namespace MyDotNetBase.Domain.Shared.Abstractions;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}
