using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Interfaces;
using MyCoffeeShop.Application.Users;
using MyCoffeeShop.Application.ShopProducts;
using MyCoffeeShop.Application.ShopOrders;
using MyCoffeeShop.Application.TransactionTypes;
using MyCoffeeShop.Application.Employees;
using MyCoffeeShop.Application.Inventories;
using MyCoffeeShop.Application.Transactions;

namespace MyCoffeeShop.Application.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService,
        IDateTime dateTime) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<TransactionType> TransactionTypes => Set<TransactionType>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<ShopOrder> ShopOrders => Set<ShopOrder>();
    public DbSet<ShopProductOrder> ShopProductOrders => Set<ShopProductOrder>();
    public DbSet<ShopProduct> ShopProducts => Set<ShopProduct>();
    public DbSet<UserCredential> UserCredentials => Set<UserCredential>();
    public DbSet<User> Users => Set<User>();

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


        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

}
