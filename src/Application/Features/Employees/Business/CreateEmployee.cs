using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;

namespace MyCoffeeShop.Application.Employees;

public record CreateEmployeeCommand(
    string? FirstName,
    string? LastName,
    IFormFile File,
    decimal? Taxes,
    decimal? SalaryBrut,
    decimal? SalaryNet
) : IRequest<EmployeeDto>;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
    }
}

internal sealed class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken
    )
    {
        string filename = request.File.FileName;
        filename = Path.GetFileName(filename);
        string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "SpatiuFisiere\\Angajati", filename);
        var stream = new FileStream(uploadfilepath, FileMode.Create);
        await request.File.CopyToAsync(stream, cancellationToken);

        var entity = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            SalaryBrut = request.SalaryBrut,
            SalaryNet = request.SalaryNet,
            Taxes = request.Taxes,
            FilePath = uploadfilepath,
        };

        await _applicationDbContext.Employees.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<EmployeeDto>(entity);
    }
}
