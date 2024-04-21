using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Products;

public record FilterProductsCommand(string? Name,
    decimal? Price,
    string? Description, bool? Active) : IRequest<List<ProductDto>>;

internal sealed class FilterProductsHandler
    : IRequestHandler<FilterProductsCommand, List<ProductDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterProductsHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(
        FilterProductsCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.Products.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Name) && u.Name.Contains(request.Name));

        if (!string.IsNullOrWhiteSpace(request.Description))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Description) && u.Description.Contains(request.Description));

        if (request.Price.HasValue)
            query = query.Where(u => u.Price == request.Price.Value);

        if (request.Active.HasValue)
            query = query.Where(u => u.Active == request.Active.Value);

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<ProductDto>>(entities);
    }
}
