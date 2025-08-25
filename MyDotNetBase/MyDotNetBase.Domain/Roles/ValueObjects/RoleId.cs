﻿namespace MyDotNetBase.Domain.Roles.ValueObjects;

public readonly record struct RoleId(Guid Value)
{
    public static RoleId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
