using MyDotNetBase.Api.Contracts.Role;
using MyDotNetBase.Application.Roles.Commands;
using MyDotNetBase.Application.Roles.Queries;

namespace MyDotNetBase.Api.Controllers;

[Route("api/roles")]
public class RolesController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public Task<IActionResult> Create(
        [FromBody] CreateRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateRoleCommand(
            request.Name,
            request.Description,
            request.Permissions);
        return SendCommandAsync(command, cancellationToken);
    }

    [HttpPut]
    public Task<IActionResult> Update(
        [FromBody] UpdateRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand(
            request.RoleId,
            request.Name,
            request.Description,
            request.Permissions);
        return SendCommandAsync(command, cancellationToken);
    }

    [HttpDelete("{roleId}")]
    public Task<IActionResult> Delete(
        [FromRoute] string roleId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand(roleId);
        return SendCommandAsync(command, cancellationToken);
    }

    [HttpGet("permissions")]
    public Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
    {
        var query = new GetAllPermissionsQuery();
        return SendQueryAsync(query, cancellationToken);
    }
}
