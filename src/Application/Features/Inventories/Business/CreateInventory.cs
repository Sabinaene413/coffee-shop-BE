using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.Inventories;

public record CreateInventoryCommand(
    long ShopProductId,
    long? MinimumLevel,
    string Description,
    long Quantity
) : IRequest<InventoryDto>;

public class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    public CreateInventoryCommandValidator()
    {
        RuleFor(v => v.ShopProductId).NotEmpty();
        RuleFor(v => v.Quantity).NotEmpty();
    }
}

internal sealed class CreateInventoryCommandHandler
    : IRequestHandler<CreateInventoryCommand, InventoryDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateInventoryCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<InventoryDto> Handle(
        CreateInventoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Inventory
        {
            ShopProductId = request.ShopProductId,
            Quantity = request.Quantity,
            MinimumLevel = request.MinimumLevel,
            Description = request.Description,
        };

        await _applicationDbContext.Inventories.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<InventoryDto>(entity);
    }
}
