using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.ShopProducts;

public record FilterShopProductsCommand(string? Name,
    decimal? Price,
    string? Description) : IRequest<List<ShopProductDto>>;

internal sealed class FilterShopProductsHandler
    : IRequestHandler<FilterShopProductsCommand, List<ShopProductDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterShopProductsHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<ShopProductDto>> Handle(
        FilterShopProductsCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.ShopProducts.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Name) && u.Name.Contains(request.Name));

        if (!string.IsNullOrWhiteSpace(request.Description))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Description) && u.Description.Contains(request.Description));

        if (request.Price.HasValue)
            query = query.Where(u => u.Price == request.Price.Value);


        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<ShopProductDto>>(entities);
    }
}
