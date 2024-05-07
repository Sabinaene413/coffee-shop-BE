using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;

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

    public CreateTransactionCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
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

        await _applicationDbContext.Transactions.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TransactionDto>(entity);
    }
}
