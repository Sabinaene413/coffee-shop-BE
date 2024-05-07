using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.EmployeePayments;

public record UpdateEmployeePaymentCommand(
    long Id,
    long EmployeeId,
    decimal Amount,
    DateTime EmployeePaymentDate
) : IRequest<EmployeePaymentDto>;

public class UpdateEmployeePaymentCommandValidator : AbstractValidator<UpdateEmployeePaymentCommand>
{
    public UpdateEmployeePaymentCommandValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.EmployeePaymentDate).NotEmpty();
    }
}

internal sealed class UpdateEmployeePaymentCommandHandler
    : IRequestHandler<UpdateEmployeePaymentCommand, EmployeePaymentDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateEmployeePaymentCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<EmployeePaymentDto> Handle(
        UpdateEmployeePaymentCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.EmployeePayments
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(EmployeePayment), request.Id);

        entity.Amount = request.Amount;
        entity.EmployeeId = request.EmployeeId;
        entity.EmployeePaymentDate = request.EmployeePaymentDate;

        _applicationDbContext.EmployeePayments.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EmployeePaymentDto>(entity);
    }
}
