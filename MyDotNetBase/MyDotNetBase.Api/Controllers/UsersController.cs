using MyDotNetBase.Application.Users.Queries;

namespace MyDotNetBase.Api.Controllers;

[Route("api/users")]
public sealed class UsersController(ISender sender) : ApiController(sender)
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        return await SendQuery(query, cancellationToken);
    }
}
