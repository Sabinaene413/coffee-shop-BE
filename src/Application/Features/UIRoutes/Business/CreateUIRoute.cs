using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Common.Interfaces;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.UIRoutes;

public record CreateUIRouteCommand(
    string UiId,
    string Path,
    string FullPath,
    string? ParentUiId,
    bool? IsLeaf
) : IRequest<UIRouteDto>;

public class CreateUIRouteCommandValidator : AbstractValidator<CreateUIRouteCommand>
{
    public CreateUIRouteCommandValidator()
    {
        RuleFor(v => v.UiId).NotEmpty();
        RuleFor(v => v.Path).NotEmpty();
        RuleFor(v => v.FullPath).NotEmpty();
    }
}

internal sealed class CreateUIRouteCommandHandler
    : IRequestHandler<CreateUIRouteCommand, UIRouteDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateUIRouteCommandHandler(
        ApplicationDbContext applicationDbContext,
        ICurrentUserService currentUserService,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<UIRouteDto> Handle(
        CreateUIRouteCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new UIRoute
        {
            UiId = request.UiId,
            Path = request.Path,
            FullPath = request.FullPath,
            ParentUiId = request.ParentUiId,
            IsLeaf = request.IsLeaf ?? false
        };

        await _applicationDbContext.UIRoutes.AddAsync(entity, cancellationToken);
        return _mapper.Map<UIRouteDto>(entity);
    }
}
