using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.EmployeePayments;

public record GetEmployeePaymentByIdCommand(long Id) : IRequest<EmployeePaymentDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetEmployeePaymentByIdCommand, EmployeePaymentDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<EmployeePaymentDto> Handle(
        GetEmployeePaymentByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.EmployeePayments.FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new NotFoundException(nameof(EmployeePayment), request.Id);

        var EmployeePaymentDto = _mapper.Map<EmployeePaymentDto>(entity);

        return (EmployeePaymentDto);
    }


}
