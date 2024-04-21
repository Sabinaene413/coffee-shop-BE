using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
