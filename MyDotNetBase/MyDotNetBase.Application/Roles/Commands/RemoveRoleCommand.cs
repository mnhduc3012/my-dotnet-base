
using MyDotNetBase.Domain.Roles.Errors;
using MyDotNetBase.Domain.Roles.ValueObjects;
using MyDotNetBase.Domain.Shared.Results;

namespace MyDotNetBase.Application.Roles.Commands;

public sealed record DeleteRoleCommand(
    string RoleId
) : ICommand;

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Role ID must be a valid GUID.");
    }
}

public sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork, IRoleRepository roleRepository)
    {
        _unitOfWork = unitOfWork;
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(Guid.Parse(request.RoleId));

        var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
            return RoleErrors.NotFound;
       
        _roleRepository.Remove(role);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

