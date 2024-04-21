using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Inventories;

public record GetInventoryByIdCommand(long Id) : IRequest<InventoryDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetInventoryByIdCommand, InventoryDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<InventoryDto> Handle(
        GetInventoryByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =await _applicationDbContext.Inventories.FirstOrDefaultAsync(x=> x.Id == request.Id)
            ?? throw new NotFoundException(nameof(Inventory), request.Id);

        return _mapper.Map<InventoryDto>(entity);
    }
}
