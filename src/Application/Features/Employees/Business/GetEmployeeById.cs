using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace MyCoffeeShop.Application.Employees;

public record GetEmployeeByIdCommand(long Id) : IRequest<(EmployeeDto, IFormFile)>;

internal sealed class GetByIdHandler : IRequestHandler<GetEmployeeByIdCommand, (EmployeeDto, IFormFile)>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public GetByIdHandler(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<(EmployeeDto, IFormFile)> Handle(
        GetEmployeeByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new NotFoundException(nameof(Employee), request.Id);

        var file = CreateIFormFile(entity.FilePath);
        var employeeDto = _mapper.Map<EmployeeDto>(entity);

        return (employeeDto, file);
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
