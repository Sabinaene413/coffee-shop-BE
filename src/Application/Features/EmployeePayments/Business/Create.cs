﻿using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using MyCoffeeShop.Application.Transactions;
using MyCoffeeShop.Application.TransactionTypes;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.EmployeePayments;

public record CreateEmployeePaymentCommand(
    long EmployeeId,
    decimal Amount,
    DateTime EmployeePaymentDate
) : IRequest<EmployeePaymentDto>;

public class CreateEmployeePaymentCommandValidator : AbstractValidator<CreateEmployeePaymentCommand>
{
    public CreateEmployeePaymentCommandValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.EmployeePaymentDate).NotEmpty();
    }
}

internal sealed class CreateEmployeePaymentCommandHandler
    : IRequestHandler<CreateEmployeePaymentCommand, EmployeePaymentDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateEmployeePaymentCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<EmployeePaymentDto> Handle(
        CreateEmployeePaymentCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new EmployeePayment
        {
            EmployeePaymentDate = request.EmployeePaymentDate,
            Amount = request.Amount,
            EmployeeId = request.EmployeeId
        };


        await _applicationDbContext.EmployeePayments.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        var employee = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken);

        var newTransaction = new Transaction
        {
            EmployeePaymentId = entity.Id,
            TotalAmount = request.Amount,
            TransactionDate = request.EmployeePaymentDate,
            TransactionTypeId = (long)TransactionTypeEnum.PLATA,
            Description = $"Plata angajat numarul {entity.Id} pentru angajatul: {employee.FirstName} {employee.LastName}",
        };
        await _applicationDbContext.Transactions.AddAsync(newTransaction, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<EmployeePaymentDto>(entity);
    }
}
