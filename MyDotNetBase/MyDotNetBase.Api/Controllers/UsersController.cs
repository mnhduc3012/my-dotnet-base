using MyDotNetBase.Application.Users.Queries;

namespace MyDotNetBase.Api.Controllers;

[Route("api/users")]
public sealed class UsersController(ISender sender) : ApiController(sender)
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(
        [FromRoute] string userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(userId);
        return await SendQueryAsync(query, cancellationToken);
    }
}
