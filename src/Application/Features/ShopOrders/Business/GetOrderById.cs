using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.ShopOrders;

public record GetOrderByIdCommand(long Id) : IRequest<ShopOrderDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetOrderByIdCommand, ShopOrderDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ShopOrderDto> Handle(
        GetOrderByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.ShopOrders.Include(x => x.ShopOrderProducts).FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new NotFoundException(nameof(ShopOrder), request.Id);

        return _mapper.Map<ShopOrderDto>(entity);
    }
}
