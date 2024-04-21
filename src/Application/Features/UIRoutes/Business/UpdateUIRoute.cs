using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.UIComponents;

namespace MyCoffeeShop.Application.UIRoutes;

public record UpdateUIRouteCommand(
    long Id,
    string UiId,
    string Name,
    string Path,
    string FullPath,
    string? ParentUiId,
    bool IsLeaf,
    bool Active
) : IRequest<UIRouteDto>;

public class UpdateUIRouteCommandValidator : AbstractValidator<UpdateUIRouteCommand>
{
    public UpdateUIRouteCommandValidator()
    {
        RuleFor(v => v.UiId).NotEmpty();
        RuleFor(v => v.Path).NotEmpty();
        RuleFor(v => v.FullPath).NotEmpty();
    }
}

internal sealed class UpdateUIRouteCommandHandler
    : IRequestHandler<UpdateUIRouteCommand, UIRouteDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateUIRouteCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<UIRouteDto> Handle(
        UpdateUIRouteCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.UIRoutes
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(UIRoute), request.Id);

        entity.UiId = request.UiId;
        entity.Name = request.Name;
        entity.Path = request.Path;
        entity.FullPath = request.FullPath;
        entity.ParentUiId = request.ParentUiId;
        entity.IsLeaf = request.IsLeaf;
        entity.Active = request.Active;

        _applicationDbContext.UIRoutes.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UIRouteDto>(entity);
    }
}
