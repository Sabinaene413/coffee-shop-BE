using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.UIComponentPermissions;

public record GetUIComponentPermissionsQuery() : IRequest<List<UIComponentPermissionDto>>;

internal sealed class GetUIComponentPermissionsQueryHandler : IRequestHandler<GetUIComponentPermissionsQuery, List<UIComponentPermissionDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetUIComponentPermissionsQueryHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public Task<List<UIComponentPermissionDto>> Handle(GetUIComponentPermissionsQuery request, CancellationToken cancellationToken)
    {
        return _applicationDbContext.UIComponentPermissions.Where(x => x.Active)
            .OrderBy(x => x.Id)
            .ProjectTo<UIComponentPermissionDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}

public class UIComponentPermissionDto : IMapFrom<UIComponentPermission>
{
    public long Id { get; set; }
    public string UIComponentId { get; set; }
    public string UISideMenuItemPermissionId { get; set; }
    public bool IsHidden { get; set; }
    public bool IsDisabled { get; set; }
}

