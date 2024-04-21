using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.ShopOrders;

public record UpdateOrderCommand(
    long Id,
    string Supplier,
    decimal Cost,
    DateTime? OrderDate,
    DateTime? ArrivalDate,
    bool Received,
    List<ShopProductDto> ShopOrderProducts,
    bool Active
) : IRequest<ShopOrderDto>;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(v => v.Supplier).NotEmpty();
        RuleFor(v => v.Cost).NotEmpty();
    }
}

internal sealed class UpdateOrderCommandHandler
    : IRequestHandler<UpdateOrderCommand, ShopOrderDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ShopOrderDto> Handle(
        UpdateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.ShopOrders.Include(x => x.ShopOrderProducts)
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(ShopOrder), request.Id);

        entity.ShopOrderProducts = request.ShopOrderProducts.Select(x => new ShopProductOrder()
        {
            ShopProductId = x.ShopProductId,
            Quantity = x.Quantity,
            Cost = x.Cost,
        }).ToList();

        entity.Supplier = request.Supplier;
        entity.Cost = request.Cost;
        entity.OrderDate = request.OrderDate;
        entity.ArrivalDate = request.ArrivalDate;
        entity.Received = request.Received;
        entity.Active = request.Active;

        _applicationDbContext.ShopOrders.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ShopOrderDto>(entity);
    }
}
