using MyDotNetBase.Api.Contracts.Users;
using MyDotNetBase.Application.Users.Commands;
using MyDotNetBase.Application.Users.Queries;

namespace MyDotNetBase.Api.Controllers;

[Route("api/user")]
public sealed class UsersController : ApiController
{
    public UsersController(ISender sender) : base(sender)
    {
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FullName,
            request.Password);
        var result = await Sender.Send(command);
        if (result.IsFailure)
            return HandleFailure(result);
        return Ok();
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
