using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Common.Constants;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UISideMenuItems;

public record FilterUISideMenuItemCommand(
    long? ParentId,
    string? Label,
    int? Order,
    bool? Active,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<List<UISideMenuItemDto>>;

public class FilterUISideMenuItemValidator : AbstractValidator<FilterUISideMenuItemCommand>
{
    public FilterUISideMenuItemValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize at least greater than or equal to 1.");
    }
}

internal sealed class FilterUISideMenuItemHandler
    : IRequestHandler<FilterUISideMenuItemCommand, List<UISideMenuItemDto>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public FilterUISideMenuItemHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<List<UISideMenuItemDto>> Handle(
        FilterUISideMenuItemCommand request,
        CancellationToken cancellationToken
    )
    {

        var query = _applicationDbContext.UISideMenuItems.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Label))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Label) && u.Label.Contains(request.Label));

        if (request.ParentId.HasValue)
            query = query.Where(u => u.ParentId == request.ParentId);

        if (request.Order.HasValue)
            query = query.Where(u => u.Order == request.Order);

        if (request.Active.HasValue)
            query = query.Where(u => u.Active == request.Active);

        // Paging
        int skip = (request.PageNumber - 1) * request.PageSize;
        query = query.Skip(skip).Take(request.PageSize);

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<UISideMenuItemDto>>(entities);
    }
}
