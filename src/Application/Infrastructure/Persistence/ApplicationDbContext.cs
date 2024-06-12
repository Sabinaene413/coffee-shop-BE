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
using MyCoffeeShop.Application.SaleProducts;
using MyCoffeeShop.Application.SaleOrders;
using MyCoffeeShop.Application.CoffeeShops;
using MyCoffeeShop.Application.EmployeePayments;

namespace MyCoffeeShop.Application.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly IHttpContextAccesorService _contextAccesorService;
    private readonly IDateTime _dateTime;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccesorService contextAccesorService,
        IDateTime dateTime) : base(options)
    {
        _contextAccesorService = contextAccesorService;
        _dateTime = dateTime;
    }
    public DbSet<CoffeeShop> CoffeeShops => Set<CoffeeShop>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<EmployeePayment> EmployeePayments => Set<EmployeePayment>();
    public DbSet<TransactionType> TransactionTypes => Set<TransactionType>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionDetail> TransactionDetails => Set<TransactionDetail>();
    public DbSet<SaleOrder> SaleOrders => Set<SaleOrder>();
    public DbSet<ShopOrder> ShopOrders => Set<ShopOrder>();
    public DbSet<ShopProductOrder> ShopProductOrders => Set<ShopProductOrder>();
    public DbSet<ShopProduct> ShopProducts => Set<ShopProduct>();
    public DbSet<SaleProduct> SaleProducts => Set<SaleProduct>();
    public DbSet<SaleProductOrder> SaleProductOrders => Set<SaleProductOrder>();
    public DbSet<UserCredential> UserCredentials => Set<UserCredential>();
    public DbSet<User> Users => Set<User>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _contextAccesorService.UserId;
                    entry.Entity.CreatedAt = _dateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _contextAccesorService.UserId;
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

        foreach (var entry in ChangeTracker.Entries<BaseLocationEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.LocationId = _contextAccesorService.LocationId;
                    entry.Entity.LocationName = _contextAccesorService.LocationName;
                    break;
                case EntityState.Modified:
                    entry.Entity.LocationId = _contextAccesorService.LocationId;
                    entry.Entity.LocationName = _contextAccesorService.LocationName;
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
        if(_contextAccesorService.LocationId != null)
        {
            builder.Entity<Transaction>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
            builder.Entity<TransactionDetail>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
            builder.Entity<Employee>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
            builder.Entity<EmployeePayment>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
            builder.Entity<Inventory>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
            builder.Entity<SaleOrder>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
            builder.Entity<ShopOrder>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
            builder.Entity<ShopProductOrder>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
            builder.Entity<ShopProduct>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
            builder.Entity<SaleProduct>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
            builder.Entity<User>().HasQueryFilter(entity => entity.LocationId == _contextAccesorService.LocationId);
        }

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

}
