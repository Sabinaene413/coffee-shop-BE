using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.SaleProducts;

public record GetSaleProductByIdCommand(long Id) : IRequest<SaleProductDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetSaleProductByIdCommand, SaleProductDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<SaleProductDto> Handle(
        GetSaleProductByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =await _applicationDbContext.SaleProducts.FirstOrDefaultAsync(x=> x.Id == request.Id)
            ?? throw new NotFoundException(nameof(SaleProduct), request.Id);

        return _mapper.Map<SaleProductDto>(entity);
    }
}
