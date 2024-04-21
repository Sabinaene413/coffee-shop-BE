using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Constants;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.UIComponents;

public record GetUIComponentByIdCommand(long Id) : IRequest<UIComponentDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetUIComponentByIdCommand, UIComponentDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<UIComponentDto> Handle(
        GetUIComponentByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity =
            await _applicationDbContext.UIComponents.FirstOrDefaultAsync(x=> x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(UIComponent), request.Id);

        return _mapper.Map<UIComponentDto>(entity);
    }
}
