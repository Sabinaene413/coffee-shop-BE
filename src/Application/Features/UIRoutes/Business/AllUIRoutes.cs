using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIRoutes;

public record AllUIRoutesCommand() : IRequest<List<SyncUIRouteDto>>;

internal sealed class AllUIRoutesHandler : IRequestHandler<AllUIRoutesCommand, List<SyncUIRouteDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public AllUIRoutesHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<SyncUIRouteDto>> Handle(
        AllUIRoutesCommand request,
        CancellationToken cancellationToken
    )
    {
        var entities = await _applicationDbContext.UIRoutes.ToListAsync(cancellationToken);
        var mapped = _mapper.Map<List<SyncUIRouteDto>>(entities);
        return mapped;
    }
}
