using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MyCoffeeShop.Application.ShopOrders;

public record FilterOrdersCommand(
    long? Id,
    string? Supplier,
    decimal? Cost,
    DateTime? OrderDate,
    DateTime? ArrivalDate,
    bool? Received) : IRequest<List<ShopOrderDto>>;

internal sealed class FilterOrdersHandler
    : IRequestHandler<FilterOrdersCommand, List<ShopOrderDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterOrdersHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<ShopOrderDto>> Handle(
        FilterOrdersCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.ShopOrders.AsQueryable();

        // Apply filters
        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);

        if (request.Supplier != null && request.Supplier != string.Empty)
            query = query.Where(u => u.Supplier.ToLower().Contains(request.Supplier.ToLower()));

        if (request.Cost.HasValue)
            query = query.Where(u => u.Cost == request.Cost.Value);

        if (request.OrderDate.HasValue)
            query = query.Where(u => u.OrderDate == request.OrderDate.Value);

        if (request.ArrivalDate.HasValue)
            query = query.Where(u => u.ArrivalDate == request.ArrivalDate.Value);

        if (request.Received.HasValue)
            query = query.Where(u => u.Received == request.Received.Value);


        var entities = await query.ToListAsync(cancellationToken);
        return entities.Select(x => new ShopOrderDto()
        {
            Id = x.Id,
            ArrivalDate = x.ArrivalDate,
            Cost = x.Cost,
            OrderDate = x.OrderDate,
            Received = x.Received,
            Supplier = x.Supplier
        }).ToList();
    }
}
