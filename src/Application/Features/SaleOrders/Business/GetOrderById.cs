using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.SaleOrders;

public record GetSaleOrderByIdCommand(long Id) : IRequest<SaleOrderDto>;

internal sealed class GetSaleByIdHandler : IRequestHandler<GetSaleOrderByIdCommand, SaleOrderDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetSaleByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<SaleOrderDto> Handle(
        GetSaleOrderByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.SaleOrders.Include(x => x.SaleOrderProducts).FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new NotFoundException(nameof(SaleOrder), request.Id);

        return _mapper.Map<SaleOrderDto>(entity);
    }
}
