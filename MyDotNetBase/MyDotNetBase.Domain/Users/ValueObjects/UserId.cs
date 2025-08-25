namespace MyDotNetBase.Domain.User.ValueObjects;

public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
