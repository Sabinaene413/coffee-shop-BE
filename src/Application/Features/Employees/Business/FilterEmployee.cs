using MediatR;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace MyCoffeeShop.Application.Employees;

public record FilterEmployeesCommand(
    long? Id,
    string? FirstName,
    string? LastName,
    decimal? Taxes,
    decimal? SalaryBrut,
    decimal? SalaryNet) : IRequest<List<EmployeeWithPhoto>>;

internal sealed class FilterEmployeesHandler
    : IRequestHandler<FilterEmployeesCommand, List<EmployeeWithPhoto>>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public FilterEmployeesHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<EmployeeWithPhoto>> Handle(
        FilterEmployeesCommand request,
        CancellationToken cancellationToken
    )
    {
        var query = _applicationDbContext.Employees.AsQueryable();

        // Apply filters

        if (request.Id.HasValue)
            query = query.Where(u => u.Id == request.Id.Value);

        if (!string.IsNullOrWhiteSpace(request.FirstName))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.FirstName) && u.FirstName.Contains(request.FirstName));

        if (!string.IsNullOrWhiteSpace(request.LastName))
            query = query.Where(u => !string.IsNullOrWhiteSpace(u.LastName) && u.LastName.Contains(request.LastName));

        if (request.Taxes.HasValue)
            query = query.Where(u => u.Taxes == request.Taxes.Value);

        if (request.SalaryBrut.HasValue)
            query = query.Where(u => u.SalaryBrut == request.SalaryBrut.Value);

        if (request.SalaryNet.HasValue)
            query = query.Where(u => u.SalaryNet == request.SalaryNet.Value);


        var entities = new List<EmployeeWithPhoto>();
        foreach (var entity in await query.ToListAsync(cancellationToken))
        {
            var file = CreateIFormFile(entity.FilePath);
            var employeeWithPhoto = new EmployeeWithPhoto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                FilePath = entity.FilePath,
                SalaryNet = entity.SalaryNet,
                SalaryBrut = entity.SalaryBrut,
                Taxes = entity.Taxes,
                LocationId = entity.LocationId,
                LocationName = entity.LocationName,
                ProfilePhoto = file
            };
            entities.Add(employeeWithPhoto);
        }
        return entities;
    }

    public IFormFile CreateIFormFile(string filePath)
    {
        // Check if file exists
        if (!File.Exists(filePath))
            return null;

        // Get the file name
        var fileName = Path.GetFileName(filePath);

        // Open the file stream
        var fileStream = new FileStream(filePath, FileMode.Open);

        // Create an IFormFile object
        var formFile = new FormFile(fileStream, 0, fileStream.Length, null, fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/octet-stream" // Set the content type appropriately
        };

        return formFile;
    }
}
