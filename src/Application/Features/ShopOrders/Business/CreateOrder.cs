﻿using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.ShopOrders;

public record CreateOrderCommand(
    string Supplier,
    decimal Cost,
    DateTime? OrderDate,
    DateTime? ArrivalDate,
    bool Received,
    List<ShopProductOrderDto> ShopOrderProducts
) : IRequest<ShopOrderDto>;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(v => v.Supplier).NotEmpty();
        RuleFor(v => v.Cost).NotEmpty();
        RuleFor(v => v.ShopOrderProducts).NotEmpty();
    }
}

internal sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, ShopOrderDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ShopOrderDto> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new ShopOrder
        {
            Supplier = request.Supplier,
            Cost = request.Cost,
            OrderDate = request.OrderDate,
            ArrivalDate = request.ArrivalDate,
            Received = request.Received,
            ShopOrderProducts = request.ShopOrderProducts.Select(x => new ShopProductOrder()
            {
                ShopProductId = x.ShopProductId,
                Quantity = x.Quantity,
                Cost = x.Cost,
            }).ToList()
        };

        await _applicationDbContext.ShopOrders.AddAsync(entity, cancellationToken);
        return _mapper.Map<ShopOrderDto>(entity);
    }
}
