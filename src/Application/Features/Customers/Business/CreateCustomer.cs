using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.Customers;

public record CreateCustomerCommand(
    string Name,
    string Email,
    string Address,
    string Telephone
) : IRequest<CustomerDto>;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
    }
}

internal sealed class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            Telephone = request.Telephone,
            Address = request.Address,
        };

        await _applicationDbContext.Customers.AddAsync(entity, cancellationToken);
        return _mapper.Map<CustomerDto>(entity);
    }
}
