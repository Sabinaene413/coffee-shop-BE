using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.Reports;

public record ShopOrdersCommand(
    DateTime? RefferenceDate,
    long ReportType = 1
) : IRequest<List<ShopOrdersDto>>;

public class ShopOrdersCommandValidator : AbstractValidator<ShopOrdersCommand>
{
    public ShopOrdersCommandValidator()
    {
    }
}

internal sealed class ShopOrdersCommandHandler
    : IRequestHandler<ShopOrdersCommand, List<ShopOrdersDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public ShopOrdersCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper,
        IHttpContextAccesorService httpContextAccesorService

    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _httpContextAccesorService = httpContextAccesorService;
    }

    public async Task<List<ShopOrdersDto>> Handle(
        ShopOrdersCommand request,
        CancellationToken cancellationToken
    )
    {
        var ShopOrdersQuerry = _applicationDbContext.ShopOrders.Include(x => x.ShopOrderProducts).ThenInclude(x => x.ShopProduct).AsNoTracking()
                            .Where(x => x.OrderDate.Value.Date <= (request.RefferenceDate ?? DateTime.Now).Date);
        switch (request.ReportType)
        {
            case (long)ReportTypeEnum.ZILNIC:
                ShopOrdersQuerry = ShopOrdersQuerry.Where(x => x.OrderDate.Value.Date >= (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-1));
                break;
            case (long)ReportTypeEnum.SAPTAMANAL:
                ShopOrdersQuerry = ShopOrdersQuerry.Where(x => x.OrderDate.Value.Date >= (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-7));
                break;
            case (long)ReportTypeEnum.LUNAR:
                ShopOrdersQuerry = ShopOrdersQuerry.Where(x => x.OrderDate.Value.Date >= (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-28));
                break;
            case (long)ReportTypeEnum.ANUAL:
                ShopOrdersQuerry = ShopOrdersQuerry.Where(x => x.OrderDate.Value.Date >= (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-360));
                break;
            default:
                break;
        }

        var ShopOrders = await ShopOrdersQuerry.OrderBy(x => x.OrderDate).ToListAsync(cancellationToken);
        var salesDto = new List<ShopOrdersDto>();

        switch (request.ReportType)
        {
            case (long)ReportTypeEnum.ZILNIC:
                foreach (var saleDay in ShopOrders.GroupBy(x => x.OrderDate.Value.Date))
                {
                    var first = saleDay.FirstOrDefault();
                    if (first is null)
                        continue;

                    var newSaleDto = new ShopOrdersDto()
                    {
                        SaleDate = first.OrderDate.Value.Date,
                        Cost = saleDay.Sum(x => x.Cost),
                        //Details = saleDay.SelectMany(x => x.ShopOrderProducts).Select(x=> x.SaleProduct).Distinct()
                        NoOfSales = saleDay.Count(),
                        NoOfItemsSold = saleDay.SelectMany(x => x.ShopOrderProducts).Select(x => x.Quantity).Count()
                    };
                    salesDto.Add(newSaleDto);
                }
                break;
            case (long)ReportTypeEnum.SAPTAMANAL:
                foreach (var saleDay in ShopOrders.GroupBy(x => x.OrderDate.Value.Date))
                {
                    var first = saleDay.FirstOrDefault();
                    if (first is null)
                        continue;

                    var newSaleDto = new ShopOrdersDto()
                    {
                        SaleDate = first.OrderDate.Value.Date,
                        Cost = saleDay.Sum(x => x.Cost),
                        //Details = saleDay.SelectMany(x => x.ShopOrderProducts).Select(x=> x.SaleProduct).Distinct()
                        NoOfSales = saleDay.Count(),
                        NoOfItemsSold = saleDay.SelectMany(x => x.ShopOrderProducts).Select(x => x.Quantity).Count()
                    };
                    salesDto.Add(newSaleDto);
                }
                break;
            case (long)ReportTypeEnum.LUNAR:

                ShopOrdersDto weekSaleDto = null;
                for (int i = 1; i <= 28; i++)
                {

                    if (i % 7 == 1)
                        weekSaleDto = new ShopOrdersDto()
                        {
                            SaleDate = (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-(28 - i + 1))
                        };

                    var sales = ShopOrders.Where(x => x.OrderDate.Value.Date == (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-(28 - i + 1))).ToList();
                    if (sales is null || !sales.Any())
                    {
                        if (i % 7 == 0)
                            salesDto.Add(weekSaleDto);
                        continue;
                    }

                    weekSaleDto.Cost += sales.Sum(x => x.Cost);
                    weekSaleDto.NoOfSales += sales.Count();
                    weekSaleDto.NoOfItemsSold += sales.SelectMany(x => x.ShopOrderProducts).Select(x => x.Quantity).Count();

                    if (i % 7 == 0)
                        salesDto.Add(weekSaleDto);
                }
                break;
            case (long)ReportTypeEnum.ANUAL:
                ShopOrdersDto monthSaleDto = null;
                for (int i = 1; i <= 360; i++)
                {
                    if (i % 28 == 1)
                        monthSaleDto = new ShopOrdersDto()
                        {
                            SaleDate = (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-(360 - i + 1))
                        };

                    var sales = ShopOrders.Where(x => x.OrderDate.Value.Date == (request.RefferenceDate ?? DateTime.Now).Date.AddDays(-(360 - i + 1))).ToList();
                    if (sales is null || !sales.Any())
                    {
                        if (i % 28 == 0)
                            salesDto.Add(monthSaleDto);
                        continue;
                    }

                    monthSaleDto.Cost += sales.Sum(x => x.Cost);
                    monthSaleDto.NoOfSales += sales.Count();
                    monthSaleDto.NoOfItemsSold += sales.SelectMany(x => x.ShopOrderProducts).Select(x => x.Quantity).Count();

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
