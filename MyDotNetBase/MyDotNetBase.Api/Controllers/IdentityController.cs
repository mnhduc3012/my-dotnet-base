using MyDotNetBase.Api.Contracts.Identity;
using MyDotNetBase.Application.Identity.Commands;

namespace MyDotNetBase.Api.Controllers;

[Route("api/identity")]
public class IdentityController(ISender sender) : ApiController(sender)
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.Username,
            request.Password);
        return await SendCommand(command, cancellationToken);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        return await SendCommand(command, cancellationToken);
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
        return await SendCommand(command, cancellationToken);
    }
}