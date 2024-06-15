using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Interfaces;
using MyCoffeeShop.Application.Features.Reports;

namespace MyCoffeeShop.Application.Transactions;

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
                int countDay = 0;
                int countWeek = 0;
                foreach (var saleDay in shopSales.GroupBy(x => x.OrderDate.Value.Date))
                {
                    countDay++;
                    countWeek++;
                    if (countWeek == 1)
                        weekSaleDto = new ShopSalesDto()
                        {
                            SaleDate = (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-(28 - countDay))
                        };

                    var first = saleDay.FirstOrDefault();
                    if (first is null)
                        continue;

                    weekSaleDto.Cost += saleDay.Sum(x => x.Cost);
                    weekSaleDto.NoOfSales += saleDay.Count();
                    weekSaleDto.NoOfItemsSold += saleDay.SelectMany(x => x.SaleOrderProducts).Select(x => x.Quantity).Count();

                    if (countWeek == 7)
                    {
                        salesDto.Add(weekSaleDto);
                        countWeek = 0;
                    }
                }
                break;
            case (long)ReportTypeEnum.ANUAL:
                ShopSalesDto monthSaleDto = null;
                int countDayIn = 0;
                int countMonth = 0;
                foreach (var saleDay in shopSales.GroupBy(x => x.OrderDate.Value.Date))
                {
                    countDayIn++;
                    countMonth++;
                    if (countMonth == 1)
                        monthSaleDto = new ShopSalesDto()
                        {
                            SaleDate = (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-(28 - countDayIn))
                        };

                    var first = saleDay.FirstOrDefault();
                    if (first is null)
                        continue;

                    monthSaleDto.Cost += saleDay.Sum(x => x.Cost);
                    monthSaleDto.NoOfSales += saleDay.Count();
                    monthSaleDto.NoOfItemsSold += saleDay.SelectMany(x => x.SaleOrderProducts).Select(x => x.Quantity).Count();

                    if (countMonth == 7)
                    {
                        salesDto.Add(monthSaleDto);
                        countMonth = 0;
                    }
                }
                break;
            default:
                break;
        }


        return salesDto;
    }


}
