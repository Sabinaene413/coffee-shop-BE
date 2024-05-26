using MediatR;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace MyCoffeeShop.Application.Employees;

public record GetEmployeeByIdCommand(long Id) : IRequest<EmployeeWithPhoto>;

internal sealed class GetByIdHandler : IRequestHandler<GetEmployeeByIdCommand, EmployeeWithPhoto>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public GetByIdHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<EmployeeWithPhoto> Handle(
        GetEmployeeByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new NotFoundException(nameof(Employee), request.Id);

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
            EmployeeTypeId = entity.EmployeeTypeId,
            LocationId = entity.LocationId,
            LocationName = entity.LocationName,
            ProfilePhoto = photoData.Item1,  // Assign photo data
            ProfilePhotoContentType = photoData.Item2 // Assign content type
        };

        return employeeWithPhoto;
    }

    public IFormFile CreateIFormFile(string filePath)
    {
        // Check if file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        // Get the file name
        var fileName = Path.GetFileName(filePath);

        // Open the file stream
        using var fileStream = new FileStream(filePath, FileMode.Open);

        // Create an IFormFile object
        var formFile = new FormFile(fileStream, 0, fileStream.Length, null, fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/octet-stream" // Set the content type appropriately
        };

        return formFile;
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
