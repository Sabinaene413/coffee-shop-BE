using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.SaleProducts;

public record FilterSaleProductsCommand(string? Name,
    decimal? Price,
    string? Description) : IRequest<List<SaleProductDto>>;

internal sealed class FilterSaleProductsHandler
    : IRequestHandler<FilterSaleProductsCommand, List<SaleProductDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterSaleProductsHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<SaleProductDto>> Handle(
        FilterSaleProductsCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.SaleProducts.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Name) && u.Name.Contains(request.Name));

        if (!string.IsNullOrWhiteSpace(request.Description))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Description) && u.Description.Contains(request.Description));

        if (request.Price.HasValue)
            query = query.Where(u => u.Price == request.Price.Value);


        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<SaleProductDto>>(entities);
    }
}
