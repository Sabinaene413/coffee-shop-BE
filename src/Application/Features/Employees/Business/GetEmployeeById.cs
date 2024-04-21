using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Employees;

public record GetEmployeeByIdCommand(long Id) : IRequest<EmployeeDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetEmployeeByIdCommand, EmployeeDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(
        GetEmployeeByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new NotFoundException(nameof(Employee), request.Id);

        return _mapper.Map<EmployeeDto>(entity);
    }
}
