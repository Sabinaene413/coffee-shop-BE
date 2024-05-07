using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.EmployeePayments;

public record FilterEmployeePaymentsCommand(
    long? Id,
    long? EmployeeId,
    decimal? Amount,
    DateTime? EmployeePaymentDate) : IRequest<List<EmployeePaymentDto>>;

internal sealed class FilterEmployeePaymentsHandler
    : IRequestHandler<FilterEmployeePaymentsCommand, List<EmployeePaymentDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterEmployeePaymentsHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<EmployeePaymentDto>> Handle(
        FilterEmployeePaymentsCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.EmployeePayments.AsQueryable();

        // Apply filters

        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);

        if (request.Amount.HasValue)
            query = query.Where(u => u.Amount == request.Amount.Value);

        if (request.EmployeeId.HasValue)
            query = query.Where(u => u.EmployeeId == request.EmployeeId.Value);

        if (request.EmployeePaymentDate.HasValue)
            query = query.Where(u => u.EmployeePaymentDate == request.EmployeePaymentDate.Value);

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<EmployeePaymentDto>>(entities);
    }
}
