using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using MyCoffeeShop.Application.Inventories;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.ShopOrders;

public record MarkOrderReceivedCommand(
    long Id
) : IRequest<ShopOrderDto>;

public class MarkOrderReceivedCommandValidator : AbstractValidator<MarkOrderReceivedCommand>
{
    public MarkOrderReceivedCommandValidator()
    {
    }
}

internal sealed class MarkOrderReceivedCommandHandler
    : IRequestHandler<MarkOrderReceivedCommand, ShopOrderDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public MarkOrderReceivedCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ShopOrderDto> Handle(
        MarkOrderReceivedCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.ShopOrders.Include(x => x.ShopOrderProducts)
                                     .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
              ?? throw new NotFoundException(nameof(ShopOrder), request.Id);

        if (entity.Received)
            throw new Exception($"Comanda {entity.Id} a fost deja primita!");
        

        entity.Received = true;
        _applicationDbContext.ShopOrders.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);


        var newInventories = entity.ShopOrderProducts.Select(y => new Inventory()
        {
            Description = $"Comanda {entity.Id} Primita",
            MinimumLevel = 10,
            Quantity = y.Quantity,
            ShopProductId = y.ShopProductId,
        }).ToList();
        await _applicationDbContext.Inventories.AddRangeAsync(newInventories, cancellationToken);

        return _mapper.Map<ShopOrderDto>(entity);
    }
}
