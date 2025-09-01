using MyDotNetBase.Domain.Roles.Entities;
using MyDotNetBase.Domain.Roles.Enums;
using MyDotNetBase.Domain.Roles.Errors;
using MyDotNetBase.Domain.Roles.ValueObjects;
using MyDotNetBase.Domain.Shared.Results;

namespace MyDotNetBase.Application.Roles.Commands;

public sealed record UpdateRoleCommand(
    string RoleId,
    string Name,
    string Description,
    List<string> Permissions
) : ICommand;

public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID must not be empty.")
            .Must(roleId => Guid.TryParse(roleId, out _))
            .WithMessage("Invalid Role ID format.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name must not be empty.")
            .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Role description must not exceed 250 characters.");
        RuleForEach(x => x.Permissions)
            .Must(permission => Enum.TryParse<Permission>(permission, out _))
            .WithMessage("Invalid permission: {PropertyValue}");
    }
}

public sealed class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(Guid.Parse(request.RoleId));

        var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
            return RoleErrors.NotFound;

        var updatedRoleOrError = Role.Update(
            role,
            request.Name,
            request.Description,
            request.Permissions);
        if (updatedRoleOrError.IsFailure)
            return updatedRoleOrError.Error;

        _roleRepository.Update(updatedRoleOrError.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
