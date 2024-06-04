using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.TransactionTypes;
using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.Transactions;

public record CreateTransactionCommand(
    long TransactionTypeId,
    decimal TotalAmount,
    string Description,
    DateTime TransactionDate
) : IRequest<TransactionDto>;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.TotalAmount).NotEmpty();
        RuleFor(x => x.TransactionDate).NotEmpty();
    }
}

internal sealed class CreateTransactionCommandHandler
    : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public CreateTransactionCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper,
        IHttpContextAccesorService httpContextAccesorService

    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _httpContextAccesorService = httpContextAccesorService;
    }

    public async Task<TransactionDto> Handle(
        CreateTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Transaction
        {
            TransactionTypeId = request.TransactionTypeId,
            TotalAmount = request.TotalAmount,
            TransactionDate = request.TransactionDate,
            Description = request.Description,
        };

        var coffeeShopBudget = await _applicationDbContext.CoffeeShops.FirstOrDefaultAsync(x => x.Id == _httpContextAccesorService.LocationId, cancellationToken);
        if (coffeeShopBudget != null)
        {
            coffeeShopBudget.Budget += entity.TotalAmount * (entity.TransactionTypeId == (long)TransactionTypeEnum.VANZARE ? 1 : -1);
            _applicationDbContext.CoffeeShops.Update(coffeeShopBudget);
        }

        await _applicationDbContext.Transactions.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TransactionDto>(entity);
    }
}
