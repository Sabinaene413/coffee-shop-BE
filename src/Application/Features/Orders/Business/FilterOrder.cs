using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Orders;

public record FilterOrdersCommand(
    long? Id,
    long? CustomerId,
    int? VatRate,
    DateTime? OrderDate, bool? Active) : IRequest<List<OrderDto>>;

internal sealed class FilterOrdersHandler
    : IRequestHandler<FilterOrdersCommand, List<OrderDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterOrdersHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> Handle(
        FilterOrdersCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.Orders.AsQueryable();

        // Apply filters
        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);

        if (request.CustomerId.HasValue)
            query = query.Where(u => u.CustomerId == request.CustomerId.Value);

        if (request.OrderDate.HasValue)
            query = query.Where(u => u.CreatedAt == request.OrderDate.Value);

        if (request.VatRate.HasValue)
            query = query.Where(u => u.VatRate == request.VatRate.Value);

        if (request.Active.HasValue)
            query = query.Where(u => u.Active == request.Active.Value);

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<OrderDto>>(entities);
    }
}
