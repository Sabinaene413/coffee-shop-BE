using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.UIRoutes;

public record GetUIRouteByIdCommand(long Id) : IRequest<UIRouteDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetUIRouteByIdCommand, UIRouteDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<UIRouteDto> Handle(
        GetUIRouteByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =await _applicationDbContext.UIRoutes.FirstOrDefaultAsync(x=> x.Id == request.Id)
            ?? throw new NotFoundException(nameof(UIRoute), request.Id);

        return _mapper.Map<UIRouteDto>(entity);
    }
}
