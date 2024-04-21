using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Customers;

public record FilterCustomersCommand(
    long? Id,
    string Name,
    string Email,
    string Address,
    string Telephone, bool? Active) : IRequest<List<CustomerDto>>;

internal sealed class FilterCustomersHandler
    : IRequestHandler<FilterCustomersCommand, List<CustomerDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterCustomersHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<CustomerDto>> Handle(
        FilterCustomersCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.Customers.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Name) && u.Name.Contains(request.Name));

        if (!string.IsNullOrWhiteSpace(request.Email))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Email) && u.Email.Contains(request.Email));

        if (!string.IsNullOrWhiteSpace(request.Address))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Address) && u.Address.Contains(request.Address));

        if (!string.IsNullOrWhiteSpace(request.Telephone))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Telephone) && u.Telephone.Contains(request.Telephone));

        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);

        if (request.Active.HasValue)
            query = query.Where(u => u.Active == request.Active.Value);

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<CustomerDto>>(entities);
    }
}
