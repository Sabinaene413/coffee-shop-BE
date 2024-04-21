using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIRoutes;

public record FilterUIRoutesCommand(string? Name,string? Path, string? FullPath, string? ParentUiId, string? UiId, bool? Active) : IRequest<List<UIRouteDto>>;

internal sealed class FilterUIRoutesHandler
    : IRequestHandler<FilterUIRoutesCommand, List<UIRouteDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterUIRoutesHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<UIRouteDto>> Handle(
        FilterUIRoutesCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.UIRoutes.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Name) && u.Name.Contains(request.Name));

        if (!string.IsNullOrWhiteSpace(request.Path))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Path) && u.Path.Contains(request.Path));

        if (!string.IsNullOrWhiteSpace(request.FullPath))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.FullPath) && u.FullPath.Contains(request.FullPath));

        if (!string.IsNullOrWhiteSpace(request.ParentUiId))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.ParentUiId) && u.ParentUiId.Contains(request.ParentUiId));

        if (!string.IsNullOrWhiteSpace(request.UiId))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.UiId) && u.UiId.Contains(request.UiId));

        if (request.Active.HasValue)
            query = query.Where(u => u.Active == request.Active.Value);

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<UIRouteDto>>(entities);
    }
}
