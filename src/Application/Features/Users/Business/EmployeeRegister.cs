using FluentValidation;
using MediatR;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using AutoMapper;
using MyCoffeeShop.Application.Common.Constants;
using MyCoffeeShop.Application.Common.Interfaces;
namespace MyCoffeeShop.Application.Users;

public record EmployeeRegisterUserCommand(
    string FirstName,
    string LastName,
    string Password,
    string Email) : IRequest<UserDto>;

public class EmployeeRegisterUserCommandValidator : AbstractValidator<EmployeeRegisterUserCommand>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public EmployeeRegisterUserCommandValidator(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        RuleFor(v => v.FirstName)
                   .MaximumLength(ConfigurationConstants.nameStringLength)
                   .NotEmpty();

        RuleFor(v => v.LastName)
            .MaximumLength(ConfigurationConstants.nameStringLength)
            .NotEmpty();

        RuleFor(v => v.Email)
            .MaximumLength(ConfigurationConstants.nameStringLength)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Adresa de email nu este valida.")
            .Must(BeUniqueEmail)
            .WithMessage("Adresa de email introdusa exista deja.");
    }

    private bool BeUniqueEmail(string email)
    {
        // Check if the email is unique in the database
        return !_applicationDbContext.Users.Any(u => u.Email == email);
    }
}

internal sealed class EmployeeRegisterUserCommandHandler : IRequestHandler<EmployeeRegisterUserCommand, UserDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public EmployeeRegisterUserCommandHandler(ApplicationDbContext context,
        IMapper mapper, IHttpContextAccesorService httpContextAccesorService)
    {
        _httpContextAccesorService = httpContextAccesorService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(EmployeeRegisterUserCommand request, CancellationToken cancellationToken)
    {
        string? passwordSalt = null;
        string? passwordHash = null;

        passwordHash = PasswordUtils.Hash(request.Password, out passwordSalt);

        var userCredentials = new UserCredential
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };
        _context.UserCredentials.Add(userCredentials);


        var coffeeShopId = _httpContextAccesorService.LocationId;

        await _context.SaveChangesAsync(cancellationToken);
        var entity = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Role = UserRole.User,
            UserCredentialId = userCredentials.Id,
            LocationId = coffeeShopId
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserDto>(entity); ;
    }
}

