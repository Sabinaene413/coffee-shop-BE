using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Products;

public record GetProductByIdCommand(long Id) : IRequest<ProductDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetProductByIdCommand, ProductDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(
        GetProductByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =await _applicationDbContext.Products.FirstOrDefaultAsync(x=> x.Id == request.Id)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        return _mapper.Map<ProductDto>(entity);
    }
}
