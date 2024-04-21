using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Common.Constants;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIComponents;
public record FilterUIComponentsCommand(
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<List<UIComponentDto>>;

public class FilterUIComponentsCommandValidator : AbstractValidator<FilterUIComponentsCommand>
{
    public FilterUIComponentsCommandValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize at least greater than or equal to 1.");
    }
}

internal sealed class FilterUIComponentsHandler
    : IRequestHandler<FilterUIComponentsCommand, List<UIComponentDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterUIComponentsHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<UIComponentDto>> Handle(
        FilterUIComponentsCommand request,
        CancellationToken cancellationToken
    )
    {
        var entities = await _applicationDbContext.UIComponents.ToListAsync(cancellationToken);
        return _mapper.Map<List<UIComponentDto>>(entities);
    }
}
