using MyDotNetBase.Api.Contracts.Identity;
using MyDotNetBase.Application.Identity.Commands;

namespace MyDotNetBase.Api.Controllers;

[Route("api/identity")]
public class IdentityController : ApiController
{
    public IdentityController(ISender sender) : base(sender)
    {
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.Username,
            request.Password);
        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure)
            return HandleFailure(result);
        return Ok(result.Value);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure)
            return HandleFailure(result);
        return Ok(result.Value);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FullName,
            request.Password);
        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure)
            return HandleFailure(result);
        return Ok();
    }
}
