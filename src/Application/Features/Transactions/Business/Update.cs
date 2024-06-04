using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.TransactionTypes;
using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.Transactions;

public record UpdateTransactionCommand(
    long Id, 
    long TransactionTypeId,
    decimal TotalAmount,
    string Description,
    DateTime TransactionDate

) : IRequest<TransactionDto>;

public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        RuleFor(x => x.TotalAmount).NotEmpty();
        RuleFor(x => x.TransactionDate).NotEmpty();
    }
}

internal sealed class UpdateTransactionCommandHandler
    : IRequestHandler<UpdateTransactionCommand, TransactionDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public UpdateTransactionCommandHandler(
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
        UpdateTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Transactions
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(Transaction), request.Id);


        var coffeeShopBudget = await _applicationDbContext.CoffeeShops.FirstOrDefaultAsync(x => x.Id == _httpContextAccesorService.LocationId, cancellationToken);
        if (coffeeShopBudget != null)
        {
            coffeeShopBudget.Budget -= entity.TotalAmount * (entity.TransactionTypeId == (long)TransactionTypeEnum.VANZARE ? 1 : -1);
            coffeeShopBudget.Budget += request.TotalAmount * (request.TransactionTypeId == (long)TransactionTypeEnum.VANZARE ? 1 : -1);
            _applicationDbContext.CoffeeShops.Update(coffeeShopBudget);
        }


        entity.TransactionDate = request.TransactionDate;
        entity.TotalAmount = request.TotalAmount;
        entity.TransactionTypeId = request.TransactionTypeId;
        entity.Description = request.Description;


        _applicationDbContext.Transactions.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TransactionDto>(entity);
    }
}
