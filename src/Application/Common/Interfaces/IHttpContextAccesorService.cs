namespace MyCoffeeShop.Application.Common.Interfaces;

public interface IHttpContextAccesorService
{
    long? UserId { get; }
    long? LocationId { get; }
    string? LocationName { get; }
}
