using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using MyCoffeeShop.Application.Common.Interfaces;
using MyCoffeeShop.Application.Features.Authentications;
using MyCoffeeShop.Application.Features.CoffeeShop;

namespace MyCoffeeShop.Application.Infrastructure.Services;

public class HttpContextAccesorService : IHttpContextAccesorService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextAccesorService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long? UserId => long.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(SystemClaimType.UID) ?? "-1");
    public long? LocationId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(Constants.LocationId) == null ? null : long.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(Constants.LocationId));
    public string? LocationName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(Constants.LocationName);

}