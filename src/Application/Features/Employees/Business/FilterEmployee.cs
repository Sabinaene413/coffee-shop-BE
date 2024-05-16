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
            var photoData = GetFileData(entity.FilePath);
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
                ProfilePhoto = photoData.Item1,  // Assign photo data
                ProfilePhotoContentType = photoData.Item2 // Assign content type
            };
            entities.Add(employeeWithPhoto);
        }
        return entities;
    }

    private (byte[], string) GetFileData(string filePath)
    {
        if (!File.Exists(filePath))
            return (null, null);

        var content = File.ReadAllBytes(filePath);
        var contentType = "application/octet-stream"; // Default content type, change if necessary

        return (content, contentType);
    }
}
