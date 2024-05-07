using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Transactions;

public record GetTransactionByIdCommand(long Id) : IRequest<TransactionDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetTransactionByIdCommand, TransactionDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(
        GetTransactionByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new NotFoundException(nameof(Transaction), request.Id);

        var TransactionDto = _mapper.Map<TransactionDto>(entity);

        return (TransactionDto);
    }


}
