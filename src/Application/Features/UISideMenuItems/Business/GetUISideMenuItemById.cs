using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UISideMenuItems;

public record GetUISideMenuItemByIdCommand(long Id) : IRequest<UISideMenuItemDto>;

internal sealed class GetByIdHandler
    : IRequestHandler<GetUISideMenuItemByIdCommand, UISideMenuItemDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<UISideMenuItemDto> Handle(
        GetUISideMenuItemByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =
            await _applicationDbContext.UISideMenuItems.Include(x=> x.UiRoute).Where(x=> x.Id == request.Id).FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(UISideMenuItem), request.Id);

        return _mapper.Map<UISideMenuItemDto>(entity);
    }
}
