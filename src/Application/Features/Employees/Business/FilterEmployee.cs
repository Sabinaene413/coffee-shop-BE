using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Employees;

public record FilterEmployeesCommand(
    long? Id,
    string Name,
    string Address,
    string ContactInformation,
    string Position,
    DateTime? BirthDate,
    DateTime? HireDate,
    bool? Active) : IRequest<List<EmployeeDto>>;

internal sealed class FilterEmployeesHandler
    : IRequestHandler<FilterEmployeesCommand, List<EmployeeDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterEmployeesHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> Handle(
        FilterEmployeesCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.Employees.AsQueryable();

        // Apply filters

        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Name) && u.Name.Contains(request.Name));

        if (!string.IsNullOrWhiteSpace(request.Address))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Address) && u.Address.Contains(request.Address));

        if (!string.IsNullOrWhiteSpace(request.Position))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Position) && u.Position.Contains(request.Position));

        if (!string.IsNullOrWhiteSpace(request.ContactInformation))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.ContactInformation) && u.ContactInformation.Contains(request.ContactInformation));

        if (request.BirthDate.HasValue)
            query = query.Where(u => u.BirthDate == request.BirthDate.Value);

        if (request.HireDate.HasValue)
            query = query.Where(u => u.HireDate == request.HireDate.Value);

        if (request.Active.HasValue)
            query = query.Where(u => u.Active == request.Active.Value);

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<EmployeeDto>>(entities);
    }
}
