using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Invoices;

public record GetInvoiceByIdCommand(long Id) : IRequest<InvoiceDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetInvoiceByIdCommand, InvoiceDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<InvoiceDto> Handle(
        GetInvoiceByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =await _applicationDbContext.Invoices.FirstOrDefaultAsync(x=> x.Id == request.Id)
            ?? throw new NotFoundException(nameof(Invoice), request.Id);

        return _mapper.Map<InvoiceDto>(entity);
    }
}
