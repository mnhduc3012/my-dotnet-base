namespace MyDotNetBase.Domain.Shared.SoftDelete;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}
