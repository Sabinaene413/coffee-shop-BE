using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Employees;

public record FilterEmployeesCommand(
    long? Id,
    string? FirstName,
    string? LastName,
    decimal? Taxes,
    decimal? SalaryBrut,
    decimal? SalaryNet) : IRequest<List<EmployeeDto>>;

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

        if (!string.IsNullOrWhiteSpace(request.FirstName))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.FirstName) && u.FirstName.Contains(request.FirstName));

        if (!string.IsNullOrWhiteSpace(request.LastName))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.LastName) && u.LastName.Contains(request.LastName));

        if (request.Taxes.HasValue)
            query = query.Where(u => u.Taxes == request.Taxes.Value);

        if (request.SalaryBrut.HasValue)
            query = query.Where(u => u.SalaryBrut == request.SalaryBrut.Value);

        if (request.SalaryNet.HasValue)
            query = query.Where(u => u.SalaryNet == request.SalaryNet.Value);


        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<EmployeeDto>>(entities);
    }
}
