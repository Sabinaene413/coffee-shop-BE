using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.ShopProducts;

public record CreateShopProductCommand(
    string Name,
    decimal Price,
    string Description
) : IRequest<ShopProductDto>;

public class CreateShopProductCommandValidator : AbstractValidator<CreateShopProductCommand>
{
    public CreateShopProductCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Price).NotEmpty();
    }
}

internal sealed class CreateShopProductCommandHandler
    : IRequestHandler<CreateShopProductCommand, ShopProductDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateShopProductCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ShopProductDto> Handle(
        CreateShopProductCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new ShopProduct
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };

        await _applicationDbContext.ShopProducts.AddAsync(entity, cancellationToken);
        return _mapper.Map<ShopProductDto>(entity);
    }
}
