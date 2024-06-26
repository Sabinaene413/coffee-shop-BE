﻿using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.Reports;

public record DashboardInitialCommand(
) : IRequest<DashboardDto>;

public class DashboardInitialCommandValidator : AbstractValidator<DashboardInitialCommand>
{
    public DashboardInitialCommandValidator()
    {
    }
}

internal sealed class DashboardInitialCommandHandler
    : IRequestHandler<DashboardInitialCommand, DashboardDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public DashboardInitialCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper,
        IHttpContextAccesorService httpContextAccesorService

    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _httpContextAccesorService = httpContextAccesorService;
    }

    public async Task<DashboardDto> Handle(
        DashboardInitialCommand request,
        CancellationToken cancellationToken
    )
    {
        var currentBudget = (await _applicationDbContext.CoffeeShops.FirstOrDefaultAsync(x => x.Id == _httpContextAccesorService.LocationId, cancellationToken))?.Budget ?? 0;
        var totalSales = await _applicationDbContext.SaleOrders.CountAsync(cancellationToken);


        DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        DateTime firstDayOfLastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
        DateTime lastDayOfLastMonth = firstDayOfLastMonth.AddMonths(1).AddDays(-1);

        var currentMonthSales = await _applicationDbContext.SaleOrders.Where(x => x.OrderDate.Value.Date >= firstDayOfMonth && x.OrderDate.Value.Date <= lastDayOfMonth).CountAsync(cancellationToken);
        var previousMonthSales = await _applicationDbContext.SaleOrders.Where(x => x.OrderDate.Value.Date >= firstDayOfLastMonth && x.OrderDate.Value.Date <= lastDayOfLastMonth).CountAsync(cancellationToken);
        var lastMonthSales = await _applicationDbContext.SaleOrders.Where(x => x.OrderDate.Value.Date >= DateTime.Now.Date.AddDays(-28)).CountAsync(cancellationToken);
        var increasePercentage = previousMonthSales != 0 ? ((float)currentMonthSales - (float)previousMonthSales) / (float)previousMonthSales * 100.0 : 0;
        var totalItemsSales = (await _applicationDbContext.SaleOrders.Include(x => x.SaleOrderProducts).ThenInclude(x => x.SaleProduct).SelectMany(x => x.SaleOrderProducts).ToListAsync(cancellationToken)).Sum(x => x.Quantity);
        var currentMonthTotalItemsSales = (await _applicationDbContext.SaleOrders.Include(x => x.SaleOrderProducts).ThenInclude(x => x.SaleProduct).Where(x => x.OrderDate.Value.Date >= firstDayOfMonth && x.OrderDate.Value.Date <= lastDayOfMonth).SelectMany(x => x.SaleOrderProducts).ToListAsync(cancellationToken)).Sum(x => x.Quantity);

        return new DashboardDto()
        {
            Budget = currentBudget,
            NoOfSales = totalSales,
            NoOfSalesCurrentMonth = currentMonthSales,
            NoOfSalesLastMonth = lastMonthSales,
            IncreasePercentageCurrentMonthSales = Math.Floor(increasePercentage),
            NoOfSelledItems = totalItemsSales,
            NoOfSelledItemsCurrentMonth = currentMonthTotalItemsSales
        };
    }


}
