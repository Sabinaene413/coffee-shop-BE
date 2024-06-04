using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Employees;
using MyCoffeeShop.Application.Common.Interfaces;

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
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public UpdateEmployeePaymentCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper,
        IHttpContextAccesorService httpContextAccesorService
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _httpContextAccesorService = httpContextAccesorService;
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

        var employee = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken);

        var oldTransaction = await _applicationDbContext.Transactions.FirstOrDefaultAsync(x => x.EmployeePaymentId == entity.Id, cancellationToken);
        if (oldTransaction != null)
        {

            var coffeeShopBudget = await _applicationDbContext.CoffeeShops.FirstOrDefaultAsync(x => x.Id == _httpContextAccesorService.LocationId, cancellationToken);
            if (coffeeShopBudget != null)
            {
                coffeeShopBudget.Budget += oldTransaction.TotalAmount;
                coffeeShopBudget.Budget -= entity.Amount;
                _applicationDbContext.CoffeeShops.Update(coffeeShopBudget);
            }

            oldTransaction.TotalAmount = entity.Amount;
            oldTransaction.TransactionDate = entity.EmployeePaymentDate;
            oldTransaction.Description = $"Plata angajat numarul {entity.Id} pentru angajatul: {employee.FirstName} {employee.LastName}";
            _applicationDbContext.Transactions.Update(oldTransaction);
        }

        _applicationDbContext.EmployeePayments.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EmployeePaymentDto>(entity);
    }
}
