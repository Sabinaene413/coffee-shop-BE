using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Transactions;

public record FilterTransactionsCommand(
    long? Id,
    long? TransactionTypeId,
    decimal? TotalAmount,
    string? Description,
    DateTime? TransactionDate
) : IRequest<List<TransactionDto>>;

internal sealed class FilterTransactionsHandler
    : IRequestHandler<FilterTransactionsCommand, List<TransactionDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterTransactionsHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<TransactionDto>> Handle(
        FilterTransactionsCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.Transactions.AsQueryable();

        // Apply filters

        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);

        if (request.TotalAmount.HasValue)
            query = query.Where(u => u.TotalAmount == request.TotalAmount.Value);

        if (request.TransactionTypeId.HasValue)
            query = query.Where(u => u.TransactionTypeId == request.TransactionTypeId.Value);

        if (request.TransactionDate.HasValue)
            query = query.Where(u => u.TransactionDate == request.TransactionDate.Value);

        if (!string.IsNullOrWhiteSpace(request.Description))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Description) && u.Description.Contains(request.Description));

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<TransactionDto>>(entities);
    }
}
