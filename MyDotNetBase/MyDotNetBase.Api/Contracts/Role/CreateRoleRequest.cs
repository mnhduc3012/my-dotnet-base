namespace MyDotNetBase.Api.Contracts.Role;

public sealed record CreateRoleRequest(
    string Name,
    string Description,
    List<string> Permissions);
