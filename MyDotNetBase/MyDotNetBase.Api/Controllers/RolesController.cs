using MyDotNetBase.Api.Contracts.Role;
using MyDotNetBase.Application.Roles.Commands;

namespace MyDotNetBase.Api.Controllers;

[Route("api/roles")]
public class RolesController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateRoleCommand(
            request.Name,
            request.Description,
            request.Permissions);
        return await SendCommandAsync(command, cancellationToken);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand(
            request.RoleId,
            request.Name,
            request.Description,
            request.Permissions);
        return await SendCommandAsync(command, cancellationToken);
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> Delete(
        [FromRoute] string roleId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand(roleId);
        return await SendCommandAsync(command, cancellationToken);
    }
}
