using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.UISideMenuItemPermissions;

public record GetUISideMenuItemsPermissionsQuery() : IRequest<List<UISideMenuItemsPermissionsDto>>;

internal sealed class GetUISideMenuItemsPermissionsQueryHandler : IRequestHandler<GetUISideMenuItemsPermissionsQuery, List<UISideMenuItemsPermissionsDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetUISideMenuItemsPermissionsQueryHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<UISideMenuItemsPermissionsDto>> Handle(GetUISideMenuItemsPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _applicationDbContext.UISideMenuItemPermissions
            .Where(x => x.Active)
            .OrderBy(x => x.Id)
            .ProjectTo<UISideMenuItemsPermissionsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}

public class UISideMenuItemsPermissionsDto : IMapFrom<UISideMenuItemPermission>
{
    public long Id { get; set; }
    public long UISideMenuItemId { get; set; }
    public long BusinessRoleId { get; set; }
    public string? BusinessUnitId { get; set; }
    public bool IsHidden { get; set; } = false;
    public bool IsDisabled { get; set; } = false;

    // public UISideMenuItemDto? UISideMenuItem { get; set; }
}

