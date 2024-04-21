using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Inventories;

public record FilterInventorysCommand(long? Id,
    long? ShopProductId,
    long? MinimumLevel,
    string Description,
    long? Quantity,
    bool? Active) : IRequest<List<InventoryDto>>;

internal sealed class FilterInventorysHandler
    : IRequestHandler<FilterInventorysCommand, List<InventoryDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterInventorysHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<InventoryDto>> Handle(
        FilterInventorysCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.Inventories.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Description))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Description) && u.Description.Contains(request.Description));

        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);

        if (request.ShopProductId.HasValue)
            query = query.Where(u => u.ShopProductId == request.ShopProductId.Value);

        if (request.MinimumLevel.HasValue)
            query = query.Where(u => u.MinimumLevel == request.MinimumLevel.Value);

        if (request.Quantity.HasValue)
            query = query.Where(u => u.Quantity == request.Quantity.Value);


        if (request.Active.HasValue)
            query = query.Where(u => u.Active == request.Active.Value);

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<InventoryDto>>(entities);
    }
}
