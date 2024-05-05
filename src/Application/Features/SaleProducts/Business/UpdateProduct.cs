using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.SaleProducts;

public record UpdateSaleProductCommand(
    long Id,
    string Name,
    decimal Price,
    string? Description
) : IRequest<SaleProductDto>;

public class UpdateSaleProductCommandValidator : AbstractValidator<UpdateSaleProductCommand>
{
    public UpdateSaleProductCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Price).NotEmpty();
    }
}

internal sealed class UpdateSaleProductCommandHandler
    : IRequestHandler<UpdateSaleProductCommand, SaleProductDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateSaleProductCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<SaleProductDto> Handle(
        UpdateSaleProductCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.SaleProducts
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(SaleProduct), request.Id);

        entity.Name = request.Name;
        entity.Price = request.Price;
        entity.Description = request.Description;

        _applicationDbContext.SaleProducts.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SaleProductDto>(entity);
    }
}
