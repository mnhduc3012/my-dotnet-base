using MyDotNetBase.Application.Users.Queries;

namespace MyDotNetBase.Api.Controllers;

[Route("api/users")]
public sealed class UsersController : ApiController
{
    public UsersController(ISender sender) : base(sender)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var result = await Sender.Send(new GetUserByIdQuery(id));
        if (result.IsFailure)
            return HandleFailure(result);
        return Ok(result.Value);
    }
}
