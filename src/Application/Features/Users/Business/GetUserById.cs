using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Users;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using MyCoffeeShop.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Features.Users.Business;

public record GetUserByIdCommand(long Id) : IRequest<UserDto>;

internal sealed class GetByIdHandler : IRequestHandler<GetUserByIdCommand, UserDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(
        GetUserByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x=> x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.Id);

        return _mapper.Map<UserDto>(entity);
    }
}
