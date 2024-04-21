using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Orders;

public record GetOrderByIdCommand(long Id) : IRequest<OrderDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetOrderByIdCommand, OrderDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(
        GetOrderByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =await _applicationDbContext.Orders.FirstOrDefaultAsync(x=> x.Id == request.Id)
            ?? throw new NotFoundException(nameof(Order), request.Id);

        return _mapper.Map<OrderDto>(entity);
    }
}
