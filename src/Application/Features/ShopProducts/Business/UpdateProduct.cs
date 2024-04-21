using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.ShopProducts;

public record UpdateShopProductCommand(
    long Id,
    string Name,
    decimal Price,
    string Description,
    bool Active
) : IRequest<ShopProductDto>;

public class UpdateShopProductCommandValidator : AbstractValidator<UpdateShopProductCommand>
{
    public UpdateShopProductCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Price).NotEmpty();
    }
}

internal sealed class UpdateShopProductCommandHandler
    : IRequestHandler<UpdateShopProductCommand, ShopProductDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateShopProductCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<ShopProductDto> Handle(
        UpdateShopProductCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.ShopProducts
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(ShopProduct), request.Id);

        entity.Name = request.Name;
        entity.Price = request.Price;
        entity.Description = request.Description;
        entity.Active = request.Active;

        _applicationDbContext.ShopProducts.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ShopProductDto>(entity);
    }
}
