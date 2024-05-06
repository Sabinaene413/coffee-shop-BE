using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.CoffeeShops;

public record UpdateCoffeeShopCommand(
    long Id,
    string Name
) : IRequest<CoffeeShopDto>;

public class UpdateCoffeeShopCommandValidator : AbstractValidator<UpdateCoffeeShopCommand>
{
    public UpdateCoffeeShopCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

internal sealed class UpdateCoffeeShopCommandHandler
    : IRequestHandler<UpdateCoffeeShopCommand, CoffeeShopDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateCoffeeShopCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<CoffeeShopDto> Handle(
        UpdateCoffeeShopCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.CoffeeShops
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(CoffeeShop), request.Id);

        entity.Name = request.Name;

        _applicationDbContext.CoffeeShops.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CoffeeShopDto>(entity);
    }
}
