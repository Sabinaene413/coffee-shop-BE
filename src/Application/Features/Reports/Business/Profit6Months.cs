using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using MyCoffeeShop.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.TransactionTypes;

namespace MyCoffeeShop.Application.Reports;

public record ProfitSixMonthsCommand(
    DateTime? RefferenceDate
) : IRequest<ProfitSixMonthsDto>;

public class ProfitSixMonthsCommandValidator : AbstractValidator<ProfitSixMonthsCommand>
{
    public ProfitSixMonthsCommandValidator()
    {
    }
}

internal sealed class ProfitSixMonthsCommandHandler
    : IRequestHandler<ProfitSixMonthsCommand, ProfitSixMonthsDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public ProfitSixMonthsCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper,
        IHttpContextAccesorService httpContextAccesorService

    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _httpContextAccesorService = httpContextAccesorService;
    }

    public async Task<ProfitSixMonthsDto> Handle(
        ProfitSixMonthsCommand request,
        CancellationToken cancellationToken
    )
    {
        var profitSixMonthsDto = new ProfitSixMonthsDto()
        {
            ProfitGraphDtos = new List<ProfitGraphDto>()
        };

        var refDate = request.RefferenceDate ?? DateTime.Now;

        for (int i = 1; i <= 6; i++)
        {

            DateTime firstDayOfLastMonth = new DateTime(refDate.Year, refDate.AddMonths(-i).Month, 1);
            if (refDate.Month < firstDayOfLastMonth.Month)
                firstDayOfLastMonth = firstDayOfLastMonth.AddYears(-1);

            DateTime lastDayOfLastMonth = firstDayOfLastMonth.AddMonths(1).AddDays(-1);

            var monthTransactions = await _applicationDbContext.Transactions.Where(x => x.TransactionDate.Value.Date >= firstDayOfLastMonth && x.TransactionDate.Value.Date <= lastDayOfLastMonth).ToListAsync(cancellationToken);
            var totalIncome = monthTransactions.Where(x => x.TransactionTypeId == (long)TransactionTypeEnum.VANZARE).Sum(x => x.TotalAmount);
            var totalExpenses = monthTransactions.Where(x => x.TransactionTypeId != (long)TransactionTypeEnum.VANZARE).Sum(x => x.TotalAmount);
            profitSixMonthsDto.Income += totalIncome;
            profitSixMonthsDto.Expenses += totalExpenses;
            profitSixMonthsDto.ProfitGraphDtos.Add(new ProfitGraphDto()
            {
                Date = firstDayOfLastMonth,
                Expenses = totalExpenses,
                Income = totalIncome
            });
        }
        profitSixMonthsDto.Profit = profitSixMonthsDto.Income - profitSixMonthsDto.Expenses;
        profitSixMonthsDto.ProfitRate = Math.Floor(profitSixMonthsDto.Income != 0 ? profitSixMonthsDto.Profit / profitSixMonthsDto.Income * 100 : 0);

        return profitSixMonthsDto;
    }


}
