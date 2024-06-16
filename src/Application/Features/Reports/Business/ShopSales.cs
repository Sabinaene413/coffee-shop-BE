using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.Reports;

public record ShopSalesCommand(
    DateTime? RefferenceDate,
    long ReportType = 1
) : IRequest<List<ShopSalesDto>>;

public class ShopSalesCommandValidator : AbstractValidator<ShopSalesCommand>
{
    public ShopSalesCommandValidator()
    {
    }
}

internal sealed class ShopSalesCommandHandler
    : IRequestHandler<ShopSalesCommand, List<ShopSalesDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public ShopSalesCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper,
        IHttpContextAccesorService httpContextAccesorService

    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _httpContextAccesorService = httpContextAccesorService;
    }

    public async Task<List<ShopSalesDto>> Handle(
        ShopSalesCommand request,
        CancellationToken cancellationToken
    )
    {
        var shopSalesQuerry = _applicationDbContext.SaleOrders.Include(x => x.SaleOrderProducts).ThenInclude(x => x.SaleProduct).AsNoTracking()
                            .Where(x => x.OrderDate.Value.Date <= (request.RefferenceDate ?? DateTime.Now).Date);
        switch (request.ReportType)
        {
            case (long)ReportTypeEnum.ZILNIC:
                shopSalesQuerry = shopSalesQuerry.Where(x => x.OrderDate.Value.Date >= (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-1));
                break;
            case (long)ReportTypeEnum.SAPTAMANAL:
                shopSalesQuerry = shopSalesQuerry.Where(x => x.OrderDate.Value.Date >= (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-7));
                break;
            case (long)ReportTypeEnum.LUNAR:
                shopSalesQuerry = shopSalesQuerry.Where(x => x.OrderDate.Value.Date >= (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-28));
                break;
            case (long)ReportTypeEnum.ANUAL:
                shopSalesQuerry = shopSalesQuerry.Where(x => x.OrderDate.Value.Date >= (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-360));
                break;
            default:
                break;
        }

        var shopSales = await shopSalesQuerry.OrderBy(x => x.OrderDate).ToListAsync(cancellationToken);
        var salesDto = new List<ShopSalesDto>();

        switch (request.ReportType)
        {
            case (long)ReportTypeEnum.ZILNIC:
                foreach (var saleDay in shopSales.GroupBy(x => x.OrderDate.Value.Date))
                {
                    var first = saleDay.FirstOrDefault();
                    if (first is null)
                        continue;

                    var newSaleDto = new ShopSalesDto()
                    {
                        SaleDate = first.OrderDate.Value.Date,
                        Cost = saleDay.Sum(x => x.Cost),
                        //Details = saleDay.SelectMany(x => x.SaleOrderProducts).Select(x=> x.SaleProduct).Distinct()
                        NoOfSales = saleDay.Count(),
                        NoOfItemsSold = saleDay.SelectMany(x => x.SaleOrderProducts).Select(x => x.Quantity).Count()
                    };
                    salesDto.Add(newSaleDto);
                }
                break;
            case (long)ReportTypeEnum.SAPTAMANAL:
                foreach (var saleDay in shopSales.GroupBy(x => x.OrderDate.Value.Date))
                {
                    var first = saleDay.FirstOrDefault();
                    if (first is null)
                        continue;

                    var newSaleDto = new ShopSalesDto()
                    {
                        SaleDate = first.OrderDate.Value.Date,
                        Cost = saleDay.Sum(x => x.Cost),
                        //Details = saleDay.SelectMany(x => x.SaleOrderProducts).Select(x=> x.SaleProduct).Distinct()
                        NoOfSales = saleDay.Count(),
                        NoOfItemsSold = saleDay.SelectMany(x => x.SaleOrderProducts).Select(x => x.Quantity).Count()
                    };
                    salesDto.Add(newSaleDto);
                }
                break;
            case (long)ReportTypeEnum.LUNAR:

                ShopSalesDto weekSaleDto = null;
                for (int i = 1; i <= 28; i++)
                {

                    if (i % 7 == 1)
                        weekSaleDto = new ShopSalesDto()
                        {
                            SaleDate = (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-(28 - i + 1))
                        };

                    var sales = shopSales.Where(x => x.OrderDate.Value.Date == (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-(28 - i + 1))).ToList();
                    if (sales is null || !sales.Any())
                    {
                        if (i % 7 == 0)
                            salesDto.Add(weekSaleDto);
                        continue;
                    }

                    weekSaleDto.Cost += sales.Sum(x => x.Cost);
                    weekSaleDto.NoOfSales += sales.Count();
                    weekSaleDto.NoOfItemsSold += sales.SelectMany(x => x.SaleOrderProducts).Select(x => x.Quantity).Count();

                    if (i % 7 == 0)
                        salesDto.Add(weekSaleDto);
                }
                break;
            case (long)ReportTypeEnum.ANUAL:
                ShopSalesDto monthSaleDto = null;
                for (int i = 1; i <= 360; i++)
                {
                    if (i % 28 == 1)
                        monthSaleDto = new ShopSalesDto()
                        {
                            SaleDate = (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-(360 - i + 1))
                        };

                    var sales = shopSales.Where(x => x.OrderDate.Value.Date == (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-(360 - i + 1))).ToList();
                    if (sales is null || !sales.Any())
                    {
                        if (i % 28 == 0)
                            salesDto.Add(monthSaleDto);
                        continue;
                    }

                    monthSaleDto.Cost += sales.Sum(x => x.Cost);
                    monthSaleDto.NoOfSales += sales.Count();
                    monthSaleDto.NoOfItemsSold += sales.SelectMany(x => x.SaleOrderProducts).Select(x => x.Quantity).Count();

                    if (i % 28 == 0)
                        salesDto.Add(monthSaleDto);
                }
                break;
            default:
                break;
        }


        return salesDto;
    }


}
