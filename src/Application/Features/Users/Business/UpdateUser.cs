using FluentValidation;
using MediatR;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using MyCoffeeShop.Application.Common.Constants;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MyCoffeeShop.Application.Users;

public class UpdateUserCommand : IRequest<UserDto>
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UpdateUserCommandValidator(ApplicationDbContext applicationDbContext)
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
            .Must((command, email) => BeUniqueEmail(email, command.Id))
            .WithMessage("Adresa de email introdusa exista deja.");
    }

    private bool BeUniqueEmail(string email, long Id)
    {
        // Check if the email is unique in the database
        return !_applicationDbContext.Users.Any(u => u.Email == email && u.Id != Id);
    }
}

internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.Id);

            var userCredential = await _context.UserCredentials.FirstOrDefaultAsync(x => x.Id == user.UserCredentialId) ?? throw new NotFoundException(nameof(User), request.Id);

            if (
                !string.IsNullOrWhiteSpace(request.Password)
            )
            {
                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    string? passwordHash = null;
                    passwordHash = PasswordUtils.Hash(request.Password, out string? passwordSalt);

                    userCredential.PasswordHash = passwordHash;
                    userCredential.PasswordSalt = passwordSalt;
                }
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.UserName;

            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}