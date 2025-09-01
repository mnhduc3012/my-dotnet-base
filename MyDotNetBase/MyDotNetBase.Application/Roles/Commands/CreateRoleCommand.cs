using MyDotNetBase.Domain.Roles.Entities;
using MyDotNetBase.Domain.Roles.Enums;

namespace MyDotNetBase.Application.Roles.Commands;

public sealed record CreateRoleCommand(
    string Name,
    string Description,
    List<string> Permissions
) : ICommand;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
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

public sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleOrError = Role.Create(
            request.Name,
            request.Description,
            request.Permissions);
        if (roleOrError.IsFailure)
            return roleOrError.Error;

        _roleRepository.Add(roleOrError.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}