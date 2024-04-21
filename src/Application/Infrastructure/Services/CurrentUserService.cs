using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long? UserId => long.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue("UserId") ?? "-1")!;

}