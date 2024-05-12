using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Users;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.Features.Users.Business;

public class FilterUsersCommand : IRequest<List<UserDto>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public long? LocationId { get; set; }
    public int PageNumber { get; set; } 
    public int PageSize { get; set; } 

    public FilterUsersCommand(
        string? firstName,
        string? lastName,
        string? email,
        string? userName,
        int pageNumber = 1,
        int pageSize = 15)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class FilterUsersCommandValidator : AbstractValidator<FilterUsersCommand>
{
    public FilterUsersCommandValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize at least greater than or equal to 1.");
    }
}

internal sealed class FilterUsersHandler : IRequestHandler<FilterUsersCommand, List<UserDto>>
{
    private readonly ApplicationDbContext _appDbContext;
    private readonly IMapper _mapper;

    public FilterUsersHandler(
        ApplicationDbContext appDbContext,
        IMapper mapper
    )
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> Handle(
        FilterUsersCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _appDbContext.Users.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.FirstName))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.FirstName) && u.FirstName.Contains(request.FirstName));

        if (!string.IsNullOrWhiteSpace(request.LastName))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.LastName) && u.LastName.Contains(request.LastName));

        if (!string.IsNullOrWhiteSpace(request.Email))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.Email) && u.Email.Contains(request.Email));

        if (!string.IsNullOrWhiteSpace(request.UserName))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.UserName) && u.UserName.Contains(request.UserName));

        if (request.LocationId.HasValue)
            query = query.Where(u => u.LocationId == request.LocationId.Value);


        // Paging
        int skip = (request.PageNumber - 1) * request.PageSize;
        query = query.Skip(skip).Take(request.PageSize);

        var entities = await query.ToListAsync(cancellationToken);

        return entities != null ?_mapper.Map<List<UserDto>>(entities) : new List<UserDto>();
    }
}
