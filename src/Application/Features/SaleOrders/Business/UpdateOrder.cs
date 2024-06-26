﻿using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Transactions;
using MyCoffeeShop.Application.TransactionTypes;
using MyCoffeeShop.Application.Common.Interfaces;

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
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public UpdateOrderCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper,
        IHttpContextAccesorService httpContextAccesorService

    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _httpContextAccesorService = httpContextAccesorService;
    }

    public async Task<SaleOrderDto> Handle(
        UpdateSaleOrderCommand request,
        CancellationToken cancellationToken
    )
    {

        var entity = await _applicationDbContext.SaleOrders.Include(x => x.SaleOrderProducts)
                                       .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(SaleOrderDto), request.Id);

        var deletedProducts = entity.SaleOrderProducts.Where(x => !(request.SaleOrderProducts.Where(y => y.Id.HasValue)?.Select(y => y.Id).ToList() ?? new List<long?>()).Contains(x.Id)).ToList();
        _applicationDbContext.SaleProductOrders.RemoveRange(deletedProducts);

        var newProducts = request.SaleOrderProducts.Where(x => !x.Id.HasValue)?.Select(x => new SaleProductOrder()
        {
            Quantity = x.Quantity,
            Price = x.Price,
            Cost = x.Cost,
        }).ToList();
        await _applicationDbContext.SaleProductOrders.AddRangeAsync(newProducts, cancellationToken);


        request.SaleOrderProducts.Where(x => x.Id.HasValue)?.ToList().ForEach(product =>
        {
            var modifiedProduct = entity.SaleOrderProducts.Where(x => x.Id == product.Id).FirstOrDefault();
            modifiedProduct.Price = product.Price;
            modifiedProduct.Cost = product.Cost;
            modifiedProduct.Quantity = product.Quantity;
        });

        entity.Cost = request.Cost;
        entity.OrderDate = request.OrderDate;

        _applicationDbContext.SaleOrders.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        
        var oldTransaction = await _applicationDbContext.Transactions.Include(x => x.TransactionDetails).FirstOrDefaultAsync(x => x.SaleOrderId == entity.Id, cancellationToken);
        entity = await _applicationDbContext.SaleOrders.Include(x => x.SaleOrderProducts).ThenInclude(x=> x.SaleProduct).FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);

        var newTransaction = new Transaction()
        {
            Description = $"Comanda vanzare numarul {entity.Id} produse: " + string.Join(", ", entity.SaleOrderProducts.Select(x => x.Quantity + "X " + x.SaleProduct.Name)),
            SaleOrderId = entity.Id,
            TotalAmount = entity.Cost,
            TransactionDate = entity.OrderDate,
            TransactionTypeId = (long)TransactionTypeEnum.VANZARE,
            TransactionDetails = entity.SaleOrderProducts.Select(x => new TransactionDetail()
            {
                SaleProductId = x.SaleProductId,
                Quantity = x.Quantity,
                Amount = x.Cost,
            }).ToList()
        };

         _applicationDbContext.Transactions.Remove(oldTransaction);

        var coffeeShopBudget = await _applicationDbContext.CoffeeShops.FirstOrDefaultAsync(x => x.Id == _httpContextAccesorService.LocationId, cancellationToken);
        if (coffeeShopBudget != null)
        {
            coffeeShopBudget.Budget -= oldTransaction.TotalAmount;
            coffeeShopBudget.Budget += newTransaction.TotalAmount;
            _applicationDbContext.CoffeeShops.Update(coffeeShopBudget);
        }
        await _applicationDbContext.Transactions.AddAsync(newTransaction, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SaleOrderDto>(entity);
    }
}
