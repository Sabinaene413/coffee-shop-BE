using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.SaleProducts;

public record CreateSaleProductCommand(
    string Name,
    decimal Price,
    string? Description
) : IRequest<SaleProductDto>;

public class CreateSaleProductCommandValidator : AbstractValidator<CreateSaleProductCommand>
{
    public CreateSaleProductCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Price).NotEmpty();
    }
}

internal sealed class CreateSaleProductCommandHandler
    : IRequestHandler<CreateSaleProductCommand, SaleProductDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateSaleProductCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<SaleProductDto> Handle(
        CreateSaleProductCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new SaleProduct
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };

        await _applicationDbContext.SaleProducts.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<SaleProductDto>(entity);
    }
}
