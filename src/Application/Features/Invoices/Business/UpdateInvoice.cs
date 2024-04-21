using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Invoices;

public record UpdateInvoiceCommand(
    long Id,
    long OrderId,
    decimal TotalAmount,
    long PaymentStatus,
    bool Active
) : IRequest<InvoiceDto>;

public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
{
    public UpdateInvoiceCommandValidator()
    {
        RuleFor(v => v.OrderId).NotEmpty();
        RuleFor(v => v.TotalAmount).NotEmpty();
        RuleFor(v => v.PaymentStatus).NotEmpty();
    }
}

internal sealed class UpdateInvoiceCommandHandler
    : IRequestHandler<UpdateInvoiceCommand, InvoiceDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateInvoiceCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<InvoiceDto> Handle(
        UpdateInvoiceCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Invoices
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(Invoice), request.Id);

        entity.OrderId = request.OrderId;
        entity.PaymentStatus = request.PaymentStatus;
        entity.TotalAmount = request.TotalAmount;
        entity.Active = request.Active;

        _applicationDbContext.Invoices.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<InvoiceDto>(entity);
    }
}
