using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.CoffeeShops;

public record GetCoffeeShopByIdCommand(long Id) : IRequest<CoffeeShopDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetCoffeeShopByIdCommand, CoffeeShopDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<CoffeeShopDto> Handle(
        GetCoffeeShopByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.CoffeeShops.FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new NotFoundException(nameof(CoffeeShop), request.Id);

        var CoffeeShopDto = _mapper.Map<CoffeeShopDto>(entity);

        return (CoffeeShopDto);
    }


}
