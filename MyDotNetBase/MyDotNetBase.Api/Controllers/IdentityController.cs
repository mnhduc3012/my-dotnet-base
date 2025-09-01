using MyDotNetBase.Api.Contracts.Identity;
using MyDotNetBase.Application.Identity.Commands;

namespace MyDotNetBase.Api.Controllers;

[Route("api/identity")]
public class IdentityController(ISender sender) : ApiController(sender)
{
    [HttpPost("login")]
    public Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.Username,
            request.Password);
        return SendCommandAsync(command, cancellationToken);
    }

    [HttpPost("refresh-token")]
    public Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        return SendCommandAsync(command, cancellationToken);
    }

    [HttpPost("register")]
    public Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FullName,
            request.Password);
        return SendCommandAsync(command, cancellationToken);
    }
}