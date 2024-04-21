using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.ShopProducts;

public record GetShopProductByIdCommand(long Id) : IRequest<ShopProductDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetShopProductByIdCommand, ShopProductDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ShopProductDto> Handle(
        GetShopProductByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =await _applicationDbContext.ShopProducts.FirstOrDefaultAsync(x=> x.Id == request.Id)
            ?? throw new NotFoundException(nameof(ShopProduct), request.Id);

        return _mapper.Map<ShopProductDto>(entity);
    }
}
