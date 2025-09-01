using MyDotNetBase.Domain.Roles.Enums;
using MyDotNetBase.Domain.Shared.Results;
using MyDotNetBase.Domain.Shared.Utilities;

namespace MyDotNetBase.Application.Roles.Queries;

public sealed record GetAllPermissionsQuery() : IQuery<GetAllPermissionsQueryResult>;
public sealed record PermissionResponse(string Value, string Description);
public sealed record GetAllPermissionsQueryResult(List<PermissionResponse> Permissions);

public sealed class GetAllPermissionsQueryHandler :
    IQueryHandler<GetAllPermissionsQuery, GetAllPermissionsQueryResult>
{
    public Task<Result<GetAllPermissionsQueryResult>> Handle(
        GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = Enum.GetValues<Permission>()
            .Select(p => new PermissionResponse(p.ToString(), p.GetDescription()))
            .ToList();
        var result = new GetAllPermissionsQueryResult(permissions);
        return Task.FromResult(Result.Success(result));
    }
}