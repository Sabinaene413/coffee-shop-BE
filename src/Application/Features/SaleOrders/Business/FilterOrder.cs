using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.SaleOrders;

public record FilterSaleOrdersCommand(
    long? Id,
    decimal? Cost,
    DateTime? OrderDate) : IRequest<List<SaleOrderDto>>;

internal sealed class FilterSaleOrdersHandler
    : IRequestHandler<FilterSaleOrdersCommand, List<SaleOrderDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterSaleOrdersHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<SaleOrderDto>> Handle(
        FilterSaleOrdersCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.SaleOrders.AsQueryable();

        // Apply filters
        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);


        if (request.Cost.HasValue)
            query = query.Where(u => u.Cost == request.Cost.Value);

        if (request.OrderDate.HasValue)
            query = query.Where(u => u.OrderDate == request.OrderDate.Value);



        var entities = await query.ToListAsync(cancellationToken);
        return entities.Select(x => new SaleOrderDto()
        {
            Id = x.Id,
            Cost = x.Cost,
            OrderDate = x.OrderDate,
            LocationId = x.LocationId,
            LocationName = x.LocationName,
        }).ToList();
    }
}
