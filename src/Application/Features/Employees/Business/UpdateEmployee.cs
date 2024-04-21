using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Employees;

public record UpdateEmployeeCommand(
    long Id,
    string Name,
    string Address,
    string ContactInformation,
    string Position,
    DateTime BirthDate,
    DateTime HireDate,
    bool Active
) : IRequest<EmployeeDto>;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Position).NotEmpty();
        RuleFor(v => v.BirthDate).NotEmpty();
        RuleFor(v => v.HireDate).NotEmpty();
    }
}

internal sealed class UpdateEmployeeCommandHandler
    : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Employees
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(Employee), request.Id);

        entity.Name = request.Name;
        entity.Address = request.Address;
        entity.ContactInformation = request.ContactInformation;
        entity.Position = request.Position;
        entity.BirthDate = request.BirthDate;
        entity.HireDate = request.HireDate;
        entity.Active = request.Active;

        _applicationDbContext.Employees.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EmployeeDto>(entity);
    }
}
