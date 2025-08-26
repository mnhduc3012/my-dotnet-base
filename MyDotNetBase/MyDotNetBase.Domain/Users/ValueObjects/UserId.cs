﻿namespace MyDotNetBase.Domain.Users.ValueObjects;

public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
