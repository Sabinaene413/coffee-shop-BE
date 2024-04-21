using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.UIComponents;

public record GetUIComponentByRouteIdCommand(long RouteId) : IRequest<List<UIComponentDto>>;

internal sealed class GetUIComponentByRouteHandler
    : IRequestHandler<GetUIComponentByRouteIdCommand, List<UIComponentDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetUIComponentByRouteHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<UIComponentDto>> Handle(
        GetUIComponentByRouteIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =
            await _applicationDbContext.UIComponents.Include(x => x.UiRoute).Where(x => x.UiRoute.Id == request.RouteId).ToListAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(UIComponent), request.RouteId);
        return _mapper.Map<List<UIComponentDto>>(entity);
    }
}
