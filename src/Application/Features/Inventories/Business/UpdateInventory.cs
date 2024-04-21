using MyCoffeeShop.Application.Infrastructure.Persistence;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Inventories;

public record UpdateInventoryCommand(
    long Id,
    long ProductId,
    long? MinimumLevel,
    DateTime ExpiryDate,
    string Description,
    long Quantity,
    bool Active
) : IRequest<InventoryDto>;

public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(v => v.ProductId).NotEmpty();
        RuleFor(v => v.Quantity).NotEmpty();
        RuleFor(v => v.ExpiryDate).NotEmpty();
    }
}

internal sealed class UpdateInventoryCommandHandler
    : IRequestHandler<UpdateInventoryCommand, InventoryDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateInventoryCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<InventoryDto> Handle(
        UpdateInventoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Inventories
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(Inventory), request.Id);

        entity.ProductId = request.ProductId;
        entity.MinimumLevel = request.MinimumLevel;
        entity.ExpiryDate = request.ExpiryDate;
        entity.Description = request.Description;
        entity.Quantity = request.Quantity;
        entity.Active = request.Active;

        _applicationDbContext.Inventories.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<InventoryDto>(entity);
    }
}
