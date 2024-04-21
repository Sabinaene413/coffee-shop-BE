﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyCoffeeShop.Application.Infrastructure.Persistence;

#nullable disable

namespace MyCoffeeShop.Application.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240408185726_AddedOrdersCustomersTransactionsProducts")]
    partial class AddedOrdersCustomersTransactionsProducts
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyCoffeeShop.Application.Customers.Customer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.Employees.Employee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ContactInformation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.Inventories.Inventory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("MinimumLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.Invoices.Invoice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("ShopOrderId")
                        .HasColumnType("bigint");

                    b.Property<long>("PaymentStatus")
                        .HasColumnType("bigint");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ShopOrderId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.ShopOrders.ShopOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<int>("VatRate")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("ShopOrders");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.ShopOrders.ShopProductOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("ShopOrderId")
                        .HasColumnType("bigint");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ShopOrderId");

                    b.ToTable("ShopProductOrder");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.ShopOrderProducts.ShopProduct", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("ShopOrderProducts");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.TransactionTypes.TransactionType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("TransactionTypes");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Active = true,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "IN"
                        },
                        new
                        {
                            Id = 2L,
                            Active = true,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "OUT"
                        });
                });

            modelBuilder.Entity("MyCoffeeShop.Application.Transactions.Transaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long?>("InvoiceId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<long?>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<long>("TransactionTypeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("TransactionTypeId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UIComponentPermissions.UIComponentPermission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<long>("UIComponentId")
                        .HasColumnType("bigint");

                    b.Property<long>("UISideMenuItemPermissionId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UIComponentId")
                        .IsUnique();

                    b.ToTable("UIComponentPermissions");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UIComponents.UIComponent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Component")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool>("HasPermissions")
                        .HasColumnType("bit");

                    b.Property<string>("UiId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UiRouteId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UiRouteId");

                    b.ToTable("UIComponents");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UIRoutes.UIRoute", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("FullPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsLeaf")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentUiId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UiId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("UIRoutes");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UISideMenuItemPermissions.UISideMenuItemPermission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<long>("BusinessRoleId")
                        .HasColumnType("bigint");

                    b.Property<string>("BusinessUnitId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<long>("UISideMenuItemId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UISideMenuItemId");

                    b.ToTable("UISideMenuItemPermissions");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UISideMenuItems.UISideMenuItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShopOrder")
                        .HasColumnType("int");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.Property<long>("UiRouteId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UiRouteId");

                    b.ToTable("UISideMenuItems");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("LastName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("UserCredentialId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Active = true,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "coffee_shop@gmail.com",
                            FirstName = "Admin",
                            LastName = "CoffeeShop",
                            Role = 1,
                            UserCredentialId = 1L,
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("MyCoffeeShop.Application.Users.UserCredential", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("UserCredentials");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Active = true,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "coffee_shop@gmail.com",
                            PasswordHash = "Gdt2F9WNp/yq4hSSQY1Uc7Lkko59qIuvoRWp0+gcW2g=",
                            PasswordSalt = "jDpi2Xu56b3C/ZSLShZlLQ=="
                        });
                });

            modelBuilder.Entity("MyCoffeeShop.Application.Inventories.Inventory", b =>
                {
                    b.HasOne("MyCoffeeShop.Application.ShopOrderProducts.ShopProduct", "ShopProduct")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShopProduct");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.Invoices.Invoice", b =>
                {
                    b.HasOne("MyCoffeeShop.Application.ShopOrders.ShopOrder", "ShopOrder")
                        .WithMany()
                        .HasForeignKey("ShopOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShopOrder");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.ShopOrders.ShopOrder", b =>
                {
                    b.HasOne("MyCoffeeShop.Application.Customers.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.ShopOrders.ShopProductOrder", b =>
                {
                    b.HasOne("MyCoffeeShop.Application.ShopOrders.ShopOrder", "ShopOrder")
                        .WithMany("ShopOrderProducts")
                        .HasForeignKey("ShopOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShopOrder");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.Transactions.Transaction", b =>
                {
                    b.HasOne("MyCoffeeShop.Application.TransactionTypes.TransactionType", "TransactionType")
                        .WithMany()
                        .HasForeignKey("TransactionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TransactionType");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UIComponentPermissions.UIComponentPermission", b =>
                {
                    b.HasOne("MyCoffeeShop.Application.UIComponents.UIComponent", "UIComponent")
                        .WithOne("UIComponentPermission")
                        .HasForeignKey("MyCoffeeShop.Application.UIComponentPermissions.UIComponentPermission", "UIComponentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyCoffeeShop.Application.UISideMenuItemPermissions.UISideMenuItemPermission", "UISideMenuItemPermission")
                        .WithOne("UIComponentPermission")
                        .HasForeignKey("MyCoffeeShop.Application.UIComponentPermissions.UIComponentPermission", "UIComponentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("UIComponent");

                    b.Navigation("UISideMenuItemPermission");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UIComponents.UIComponent", b =>
                {
                    b.HasOne("MyCoffeeShop.Application.UIRoutes.UIRoute", "UiRoute")
                        .WithMany()
                        .HasForeignKey("UiRouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UiRoute");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UISideMenuItemPermissions.UISideMenuItemPermission", b =>
                {
                    b.HasOne("MyCoffeeShop.Application.UISideMenuItems.UISideMenuItem", "UISideMenuItem")
                        .WithMany()
                        .HasForeignKey("UISideMenuItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UISideMenuItem");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UISideMenuItems.UISideMenuItem", b =>
                {
                    b.HasOne("MyCoffeeShop.Application.UIRoutes.UIRoute", "UiRoute")
                        .WithMany()
                        .HasForeignKey("UiRouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UiRoute");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.ShopOrders.ShopOrder", b =>
                {
                    b.Navigation("ShopOrderProducts");
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UIComponents.UIComponent", b =>
                {
                    b.Navigation("UIComponentPermission")
                        .IsRequired();
                });

            modelBuilder.Entity("MyCoffeeShop.Application.UISideMenuItemPermissions.UISideMenuItemPermission", b =>
                {
                    b.Navigation("UIComponentPermission")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
