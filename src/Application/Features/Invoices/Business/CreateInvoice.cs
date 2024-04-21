using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.Invoices;

public record CreateInvoiceCommand(
    long OrderId,
    decimal TotalAmount,
    long PaymentStatus
) : IRequest<InvoiceDto>;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(v => v.OrderId).NotEmpty();
        RuleFor(v => v.TotalAmount).NotEmpty();
        RuleFor(v => v.PaymentStatus).NotEmpty();
    }
}

internal sealed class CreateInvoiceCommandHandler
    : IRequestHandler<CreateInvoiceCommand, InvoiceDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateInvoiceCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<InvoiceDto> Handle(
        CreateInvoiceCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Invoice
        {
            OrderId = request.OrderId,
            PaymentStatus = request.PaymentStatus,
            TotalAmount = request.TotalAmount
        };

        await _applicationDbContext.Invoices.AddAsync(entity, cancellationToken);
        return _mapper.Map<InvoiceDto>(entity);
    }
}
