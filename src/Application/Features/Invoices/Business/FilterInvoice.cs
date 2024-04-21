using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Invoices;

public record FilterInvoicesCommand(
    long? Id,
    long? OrderId,
    decimal? TotalAmount,
    long? PaymentStatus, bool? Active) : IRequest<List<InvoiceDto>>;

internal sealed class FilterInvoicesHandler
    : IRequestHandler<FilterInvoicesCommand, List<InvoiceDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterInvoicesHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<InvoiceDto>> Handle(
        FilterInvoicesCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.Invoices.AsQueryable();

        // Apply filters

        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);

        if (request.OrderId.HasValue)
            query = query.Where(u => u.OrderId == request.OrderId.Value);

        if (request.TotalAmount.HasValue)
            query = query.Where(u => u.TotalAmount == request.TotalAmount.Value);

        if (request.PaymentStatus.HasValue)
            query = query.Where(u => u.PaymentStatus == request.PaymentStatus.Value);

        if (request.Active.HasValue)
            query = query.Where(u => u.Active == request.Active.Value);

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<InvoiceDto>>(entities);
    }
}
