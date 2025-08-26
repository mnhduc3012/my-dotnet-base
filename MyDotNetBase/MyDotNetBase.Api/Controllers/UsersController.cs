using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDotNetBase.Api.Abstractions;
using MyDotNetBase.Api.Contracts.Users;
using MyDotNetBase.Application.Users.Commands.RegisterUser;

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


}
