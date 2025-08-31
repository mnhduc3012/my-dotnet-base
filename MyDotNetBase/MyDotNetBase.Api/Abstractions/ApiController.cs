using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Api.Abstractions;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender) => Sender = sender;

    protected IActionResult HandleFailure(Result result) =>
    result switch
    {
        { IsSuccess: true } =>
            throw new InvalidOperationException("Cannot handle a successful result"),

        { Error: ValidationError validationError } =>
            BadRequest(CreateProblemDetails(
                "Validation Error",
                StatusCodes.Status400BadRequest,
                validationError,
                validationError.Errors)),

        { Error.Code: "Unauthorized" } =>
            Unauthorized(CreateProblemDetails(
                "Unauthorized",
                StatusCodes.Status401Unauthorized,
                result.Error)),

        _ =>
            BadRequest(CreateProblemDetails(
                "Bad Request",
                StatusCodes.Status400BadRequest,
                result.Error))
    };


    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null)
    {
        var details = new ProblemDetails
        {
            Title = title,
            Type = error.Code,
            Detail = error.Description,
            Status = status
        };

        if (errors is { Length: > 0 })
        {
            details.Extensions[nameof(errors)] = errors;
        }

        return details;
    }
}
