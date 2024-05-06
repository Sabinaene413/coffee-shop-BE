using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.CoffeeShops;

public record FilterCoffeeShopsCommand(
    long? Id,
    string? Name) : IRequest<List<CoffeeShopDto>>;

internal sealed class FilterCoffeeShopsHandler
    : IRequestHandler<FilterCoffeeShopsCommand, List<CoffeeShopDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterCoffeeShopsHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<CoffeeShopDto>> Handle(
        FilterCoffeeShopsCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.CoffeeShops.AsQueryable();

        // Apply filters

        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Name) && u.Name.Contains(request.Name));

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<CoffeeShopDto>>(entities);
    }
}
