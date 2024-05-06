using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Inventories;

namespace MyCoffeeShop.Application.SaleOrders;

public record UpdateSaleOrderCommand(
    long Id,
    decimal Cost,
    DateTime? OrderDate,
    List<SaleProductOrderDto> SaleOrderProducts
) : IRequest<SaleOrderDto>;

public class UpdateSaleOrderCommandValidator : AbstractValidator<UpdateSaleOrderCommand>
{
    public UpdateSaleOrderCommandValidator()
    {
        RuleFor(v => v.Cost).NotEmpty();
    }
}

internal sealed class UpdateOrderCommandHandler
    : IRequestHandler<UpdateSaleOrderCommand, SaleOrderDto>
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

    public async Task<SaleOrderDto> Handle(
        UpdateSaleOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.SaleOrders.Include(x => x.SaleOrderProducts)
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(SaleOrder), request.Id);

        entity.SaleOrderProducts = request.SaleOrderProducts.Select(x => new SaleProductOrder()
        {
            SaleProductId = x.SaleProductId,
            Quantity = x.Quantity,
            Cost = x.Cost,
        }).ToList();

        entity.Cost = request.Cost;
        entity.OrderDate = request.OrderDate;

        _applicationDbContext.SaleOrders.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);



        return _mapper.Map<SaleOrderDto>(entity);
    }
}
