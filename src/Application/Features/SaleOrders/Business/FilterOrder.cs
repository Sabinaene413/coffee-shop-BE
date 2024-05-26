using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.SaleOrders;

public record FilterSaleOrdersCommand(
    long? Id,
    decimal? Cost,
    DateTime? OrderDate) : IRequest<List<SaleOrderFilterResponse>>;

internal sealed class FilterSaleOrdersHandler
    : IRequestHandler<FilterSaleOrdersCommand, List<SaleOrderFilterResponse>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterSaleOrdersHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<SaleOrderFilterResponse>> Handle(
        FilterSaleOrdersCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.SaleOrders
                                    .Include(x => x.SaleOrderProducts)
                                    .ThenInclude(x => x.SaleProduct).AsQueryable();
        // Apply filters
        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);


        if (request.Cost.HasValue)
            query = query.Where(u => u.Cost == request.Cost.Value);

        if (request.OrderDate.HasValue)
            query = query.Where(u => u.OrderDate == request.OrderDate.Value);



        var entities = await query.ToListAsync(cancellationToken);
        return entities.Select(x => new SaleOrderFilterResponse()
        {
            Id = x.Id,
            Cost = x.Cost,
            OrderDate = x.OrderDate,
            LocationId = x.LocationId,
            LocationName = x.LocationName,
            Products = $"Produse: " + string.Join(", ", x.SaleOrderProducts.Select(x => x.Quantity + "X " + x.SaleProduct.Name)),
        }).ToList();
    }
}
