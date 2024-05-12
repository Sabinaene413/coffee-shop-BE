using FluentValidation;
using MediatR;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using AutoMapper;
using MyCoffeeShop.Application.Common.Constants;
using MyCoffeeShop.Application.CoffeeShops;
namespace MyCoffeeShop.Application.Users;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Password,
    string Email,
    string LocationName,
    bool IsAdmin = true) : IRequest<UserDto>;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public RegisterUserCommandValidator(ApplicationDbContext applicationDbContext)
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

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(ApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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

        var coffeeShop = new CoffeeShop()
        {
            Name = request.LocationName
        };

        _context.CoffeeShops.Add(coffeeShop);
        _context.UserCredentials.Add(userCredentials);

        await _context.SaveChangesAsync(cancellationToken);

        var entity = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Role = !request.IsAdmin ? UserRole.User : UserRole.Admin,
            UserCredentialId = userCredentials.Id,
            LocationId = coffeeShop.Id
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserDto>(entity); ;
    }
}

