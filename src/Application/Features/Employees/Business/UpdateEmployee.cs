using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace MyCoffeeShop.Application.Employees;

public record UpdateEmployeeCommand(
    long Id,
    string? FirstName,
    string? LastName,
    IFormFile? File,
    decimal? Taxes,
    decimal? SalaryBrut,
    decimal? SalaryNet
) : IRequest<EmployeeDto>;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
    }
}

internal sealed class UpdateEmployeeCommandHandler
    : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Employees
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(Employee), request.Id);

        string uploadfilepath = null;
        if (request.File != null)
        {
            string filename = request.File.FileName;
            filename = Path.GetFileName(filename);
            uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "SpatiuFisiere\\Angajati", filename);
            // Check if the directory exists, if not, create it
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "SpatiuFisiere\\Angajati")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "SpatiuFisiere\\Angajati"));

            using var stream = new FileStream(uploadfilepath, FileMode.Create);
            await request.File.CopyToAsync(stream, cancellationToken);
        }
        entity.FilePath = uploadfilepath;
        entity.SalaryNet = request.SalaryNet;
        entity.SalaryBrut = request.SalaryBrut;
        entity.Taxes = request.Taxes;
        entity.LastName = request.LastName;
        entity.FirstName = request.FirstName;

        _applicationDbContext.Employees.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EmployeeDto>(entity);
    }
}
