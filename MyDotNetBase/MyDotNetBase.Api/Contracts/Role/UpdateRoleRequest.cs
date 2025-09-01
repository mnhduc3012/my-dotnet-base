namespace MyDotNetBase.Api.Contracts.Role;

public sealed record UpdateRoleRequest(
    string RoleId,
    string Name,
    string Description,
    List<string> Permissions);
