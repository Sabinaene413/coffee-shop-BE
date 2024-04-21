using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Customers;

public record GetCustomerByIdCommand(long Id) : IRequest<CustomerDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetCustomerByIdCommand, CustomerDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(
        GetCustomerByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =await _applicationDbContext.Customers.FirstOrDefaultAsync(x=> x.Id == request.Id)
            ?? throw new NotFoundException(nameof(Customer), request.Id);

        return _mapper.Map<CustomerDto>(entity);
    }
}
