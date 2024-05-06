using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;

namespace MyCoffeeShop.Application.CoffeeShops;

public record CreateCoffeeShopCommand(
    string Name
) : IRequest<CoffeeShopDto>;

public class CreateCoffeeShopCommandValidator : AbstractValidator<CreateCoffeeShopCommand>
{
    public CreateCoffeeShopCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

internal sealed class CreateCoffeeShopCommandHandler
    : IRequestHandler<CreateCoffeeShopCommand, CoffeeShopDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateCoffeeShopCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<CoffeeShopDto> Handle(
        CreateCoffeeShopCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new CoffeeShop
        {
            Name = request.Name,
        };

        await _applicationDbContext.CoffeeShops.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CoffeeShopDto>(entity);
    }
}
