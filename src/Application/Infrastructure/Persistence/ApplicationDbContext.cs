using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Interfaces;
using MyCoffeeShop.Application.Users;
using MyCoffeeShop.Application.UIRoutes;
using MyCoffeeShop.Application.UISideMenuItems;
using MyCoffeeShop.Application.UISideMenuItemPermissions;
using MyCoffeeShop.Application.UIComponents;
using MyCoffeeShop.Application.UIComponentPermissions;
using MyCoffeeShop.Application.Products;
using MyCoffeeShop.Application.Orders;
using MyCoffeeShop.Application.TransactionTypes;
using MyCoffeeShop.Application.Employees;
using MyCoffeeShop.Application.Invoices;
using MyCoffeeShop.Application.Customers;
using MyCoffeeShop.Application.Inventories;
using MyCoffeeShop.Application.Transactions;

namespace MyCoffeeShop.Application.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IDomainEventService _domainEventService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService,
        IDomainEventService domainEventService,
        IDateTime dateTime) : base(options)
    {
        _currentUserService = currentUserService;
        _domainEventService = domainEventService;
        _dateTime = dateTime;
    }
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<TransactionType> TransactionTypes => Set<TransactionType>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<UserCredential> UserCredentials => Set<UserCredential>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UIComponent> UIComponents => Set<UIComponent>();
    public DbSet<UIComponentPermission> UIComponentPermissions => Set<UIComponentPermission>();
    public DbSet<UIRoute> UIRoutes => Set<UIRoute>();
    public DbSet<UISideMenuItem> UISideMenuItems => Set<UISideMenuItem>();
    public DbSet<UISideMenuItemPermission> UISideMenuItemPermissions => Set<UISideMenuItemPermission>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.CreatedAt = _dateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _currentUserService.UserId;
                    entry.Entity.UpdatedAt = _dateTime.Now;
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                default:
                    break;
            }
        }

        var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var oneEvent in events)
        {
            oneEvent.IsPublished = true;
            await _domainEventService.Publish(oneEvent);
        }
    }
}
