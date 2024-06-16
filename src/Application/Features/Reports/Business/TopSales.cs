using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.Reports;

public record TopSalesCommand(
) : IRequest<List<TopSalesDto>>;

public class TopSalesCommandValidator : AbstractValidator<TopSalesCommand>
{
    public TopSalesCommandValidator()
    {
    }
}

internal sealed class TopSalesCommandHandler
    : IRequestHandler<TopSalesCommand, List<TopSalesDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public TopSalesCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper,
        IHttpContextAccesorService httpContextAccesorService

    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _httpContextAccesorService = httpContextAccesorService;
    }

    public async Task<List<TopSalesDto>> Handle(
        TopSalesCommand request,
        CancellationToken cancellationToken
    )
    {
        var totalSales = await _applicationDbContext.SaleProductOrders.Include(x => x.SaleProduct).GroupBy(x => x.SaleProduct.Name)
                        .Select(g => new
                        {
                            ProductName = g.Key,
                            TotalSales = g.Sum(y => y.Quantity)
                        }).OrderByDescending(x=> x.TotalSales).Take(5).ToListAsync(cancellationToken);

        return totalSales.Select(x => new TopSalesDto()
        {
            ProductName = x.ProductName,
            TotalSales = x.TotalSales
        }).ToList();
    }


}
