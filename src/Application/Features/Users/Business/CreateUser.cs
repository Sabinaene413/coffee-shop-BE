using FluentValidation;
using MediatR;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using AutoMapper;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Constants;
namespace MyCoffeeShop.Application.Users;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Password,
    string Email,
    string UserName,
    int RoleId) : IRequest<UserDto>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CreateUserCommandValidator(ApplicationDbContext applicationDbContext)
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

internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(ApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
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
        await _context.SaveChangesAsync(cancellationToken);

        var entity = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            Role = request.RoleId == (int)UserRole.User ? UserRole.User : UserRole.Admin,
            UserCredentialId = userCredentials.Id
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserDto>(entity); ;
    }
}

