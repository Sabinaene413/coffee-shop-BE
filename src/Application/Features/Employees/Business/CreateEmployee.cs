using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.Employees;

public record CreateEmployeeCommand(
    string Name,
    string Address,
    string ContactInformation,
    string Position,
    DateTime BirthDate,
    DateTime HireDate
) : IRequest<EmployeeDto>;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Position).NotEmpty();
        RuleFor(v => v.BirthDate).NotEmpty();
        RuleFor(v => v.HireDate).NotEmpty();
    }
}

internal sealed class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Employee
        {
            Name = request.Name,
            Address = request.Address,
            ContactInformation = request.ContactInformation,
            BirthDate = request.BirthDate,
            HireDate = request.HireDate,
            Position = request.Position
        };

        await _applicationDbContext.Employees.AddAsync(entity, cancellationToken);
        return _mapper.Map<EmployeeDto>(entity);
    }
}
