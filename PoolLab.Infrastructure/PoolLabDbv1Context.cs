using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PoolLab.Core.Models;

namespace PoolLab.Infrastructure;

public partial class PoolLabDbv1Context : DbContext
{
    public PoolLabDbv1Context()
    {
    }

    public PoolLabDbv1Context(DbContextOptions<PoolLabDbv1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<BilliardPrice> BilliardPrices { get; set; }

    public virtual DbSet<BilliardTable> BilliardTables { get; set; }

    public virtual DbSet<BilliardType> BilliardTypes { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<GroupProduct> GroupProducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Transaction> Payments { get; set; }

    public virtual DbSet<PlayTime> PlayTimes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<RegisteredCourse> RegisteredCourses { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<SubscriptionType> SubscriptionTypes { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<ConfigTable> ConfigTable { get; set; }

    public virtual DbSet<Voucher> Voucher { get; set; }

    public virtual DbSet<AccountVoucher> AccountVoucher { get; set; }

    public virtual DbSet<MentorInfo> MentorInfos { get; set; }

    public virtual DbSet<BilliardTypeArea> BilliardTypeAreas { get; set; }

    public virtual DbSet<TableIssues> TableIssues { get; set; }

    public virtual DbSet<TableMaintenance> TableMaintenances { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = GetConnectionString();
        optionsBuilder.UseSqlServer(connectionString);
    }

    public string GetConnectionString()
    {
        string connectionString;
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        connectionString = config.GetConnectionString("PhucDatabase");
        return connectionString;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AvatarUrl).HasMaxLength(100);
            entity.Property(e => e.Balance).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.JoinDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Rank).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SubId).HasColumnName("SubID");
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.TimeTotal).HasPrecision(0);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Account_Role")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Store).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Account_Store")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Sub).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.SubId)
                .HasConstraintName("FK_Account_Subscription")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.ToTable("Area");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descript).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.AreaImg).HasMaxLength(120);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
        });

        modelBuilder.Entity<BilliardPrice>(entity =>
        {
            entity.ToTable("BilliardPrice");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descript).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NewPrice).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.OldPrice).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TimeEnd).HasColumnType("datetime");
            entity.Property(e => e.TimeStart).HasColumnType("datetime");
        });

        modelBuilder.Entity<BilliardTable>(entity =>
        {
            entity.ToTable("BilliardTable");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.BilliardTypeId).HasColumnName("BilliardTypeID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Descript).HasMaxLength(1000);
            entity.Property(e => e.Image).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PriceId).HasColumnName("PriceID");
            entity.Property(e => e.Qrcode).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Area).WithMany(p => p.BilliardTables)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_BilliardTable_Area")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.BilliardType).WithMany(p => p.BilliardTables)
                .HasForeignKey(d => d.BilliardTypeId)
                .HasConstraintName("FK_BilliardTable_BilliardType")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Price).WithMany(p => p.BilliardTables)
                .HasForeignKey(d => d.PriceId)
                .HasConstraintName("FK_BilliardTable_BilliardPrice")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Store).WithMany(p => p.BilliardTables)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_BilliardTable_Store")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<BilliardType>(entity =>
        {
            entity.ToTable("BilliardType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descript).HasMaxLength(400);
            entity.Property(e => e.Image).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Booking");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BilliardTableId).HasColumnName("BilliardTableID");
            entity.Property(e => e.BilliardTypeId).HasColumnName("BilliardTypeID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.DayOfWeek).HasMaxLength(200);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStart).HasPrecision(0);
            entity.Property(e => e.TimeEnd).HasPrecision(0);
            entity.Property(e => e.Deposit).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.RecurringId).HasColumnName("RecurringId");
            entity.Property(e => e.IsRecurring).HasColumnType("BIT").HasDefaultValue(false);

            entity.HasOne(d => d.BilliardTable).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BilliardTableId)
                .HasConstraintName("FK_Booking_BilliardTable")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.BilliardType).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BilliardTypeId)
                .HasConstraintName("FK_Booking_BilliardType")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Booking_Account")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Store).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Booking_Store")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.ConfigTable).WithMany(p => p.Bookings)
               .HasForeignKey(d => d.ConfigId)
               .HasConstraintName("FK_Booking_ConfigTable")
               .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Area).WithMany(p => p.Bookings)
               .HasForeignKey(d => d.AreaId)
               .HasConstraintName("FK_Booking_Area")
               .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Recurring).WithMany(p => p.RecurringBookings)
            .HasForeignKey(d => d.RecurringId)
            .HasConstraintName("FK_Booking_Recurring")
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<BilliardTypeArea>(entity =>
        {
            entity.ToTable("BilliardTypeArea");
            entity.Property(e => e.AreaID).HasColumnName("AreaID");
            entity.Property(e => e.BilliardTypeID).HasColumnName("BilliardTypeID");
            entity.Property(e => e.StoreID).HasColumnName("StoreID");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Area).WithMany(p => p.BilliardTypeAreas)
               .HasForeignKey(d => d.AreaID)
               .HasConstraintName("FK_BilliardTypeArea_Area")
               .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.BilliardType).WithMany(p => p.BilliardTypeAreas)
               .HasForeignKey(d => d.BilliardTypeID)
               .HasConstraintName("FK_BilliardTypeArea_BilliardType")
               .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Store).WithMany(p => p.BilliardTypeAreas)
              .HasForeignKey(d => d.StoreID)
              .HasConstraintName("FK_BilliardTypeArea_Store")
              .OnDelete(DeleteBehavior.NoAction);

        });

        modelBuilder.Entity<ConfigTable>(entity =>
        {
            entity.ToTable("ConfigTable");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(400);
            entity.Property(e => e.CompanyImg).HasMaxLength(100);
            entity.Property(e => e.Descript).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Descript).HasColumnType("nvarchar(MAX)");
            entity.Property(e => e.Level).HasMaxLength(50);
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Schedule).HasMaxLength(100);

            entity.HasOne(d => d.Store).WithMany(p => p.Courses)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Course_Store")
                .OnDelete(DeleteBehavior.NoAction);


            entity.HasOne(d => d.MentorInfo).WithMany(p => p.Courses)
                .HasForeignKey(d => d.MentorId)
                .HasConstraintName("FK_Course_MentorInfo")
                .OnDelete(DeleteBehavior.NoAction);


            entity.HasOne(d => d.Account).WithMany(p => p.Courses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Course_Account")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Event");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Thumbnail).HasMaxLength(150);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.TimeEnd).HasColumnType("datetime");
            entity.Property(e => e.TimeStart).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Descript).HasColumnType("nvarchar(MAX)");


            entity.HasOne(d => d.Manager).WithMany(p => p.Events)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Event_Account")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Store).WithMany(p => p.Events)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Event_Store")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<GroupProduct>(entity =>
        {
            entity.ToTable("GroupProduct");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descript).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeId");

            entity.HasOne(d => d.ProductType).WithMany(p => p.GroupProducts)
                .HasForeignKey(d => d.ProductTypeId)
                .HasConstraintName("FK_GroupProducts_ProductType")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.OrderCode).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(100);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.StoreId).HasColumnName("StoreId");
            entity.Property(e => e.BilliardTableId).HasColumnName("BilliardTableId");
            entity.Property(e => e.PlayTimeId).HasColumnName("PlayTimeId");
            entity.Property(e => e.Discount).HasColumnType("decimal(11, 1)");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(100);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.CustomerPay).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.ExcessCash).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.AdditionalFee).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.OrderBy).HasMaxLength(100);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Order_Account")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Order_Store")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.BilliardTable).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BilliardTableId)
                .HasConstraintName("FK_Order_BilliardTable")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.PlayTime).WithMany(p => p.Orders)
              .HasForeignKey(d => d.PlayTimeId)
              .HasConstraintName("FK_Order_PlayTime")
              .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.TableIssues).WithMany(p => p.Orders)
             .HasForeignKey(d => d.TableIssuesId)
             .HasConstraintName("FK_Order_TableIssues.")
             .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.BilliardTableId).HasColumnName("BilliardTableID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.BilliardTable).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.BilliardTableId)
                .HasConstraintName("FK_OrderDetail_BilliardTable");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetail_Order")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderDetail_Product")
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Amount).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentInfo).HasMaxLength(200);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SubId).HasColumnName("SubID");
            entity.Property(e => e.TableIssuesId).HasColumnName("TableIssuesId");

            entity.HasOne(d => d.Account).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Transaction_Account");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Transaction_Order");

            entity.HasOne(d => d.Sub).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SubId)
                .HasConstraintName("FK_Transaction_Subscription");

            entity.HasOne(d => d.TableIssues).WithMany(p => p.Transactions)
             .HasForeignKey(d => d.TableIssuesId)
             .HasConstraintName("FK_Transaction_TableIssues");
        });

        modelBuilder.Entity<PlayTime>(entity =>
        {
            entity.ToTable("PlayTime");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.BilliardTableId).HasColumnName("BilliardTableID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.TotalTime).HasColumnType("decimal(11, 5)");
            entity.Property(e => e.TimeStart).HasColumnType("datetime");
            entity.Property(e => e.TimeEnd).HasColumnType("datetime");

            entity.HasOne(d => d.BilliardTable).WithMany(p => p.PlayTimes)
                .HasForeignKey(d => d.BilliardTableId)
                .HasConstraintName("FK_PlayTime_BilliardTable");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Descript).HasMaxLength(3000);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.ProductGroupId).HasColumnName("ProductGroupID");
            entity.Property(e => e.ProductImg).HasMaxLength(100);
            entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.UnitId).HasColumnName("UnitID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.ProductGroup).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductGroupId)
                .HasConstraintName("FK_Product_GroupProduct");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .HasConstraintName("FK_Product_ProductType");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK_Product_Unit");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.ToTable("ProductType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descript).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<RegisteredCourse>(entity =>
        {
            entity.ToTable("RegisteredCourse");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasPrecision(0);
            entity.Property(e => e.EndTime).HasPrecision(0);
            entity.Property(e => e.Price).HasColumnType("decimal(11,0)");
            entity.Property(e => e.Schedule).HasMaxLength(100);
            entity.Property(e => e.EnrollCourseId).HasColumnName("EnrollCourseId");
            entity.Property(e => e.IsRegistered).HasColumnType("BIT").HasDefaultValue(false);

            entity.HasOne(d => d.Course).WithMany(p => p.RegisteredCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_RegisteredCourse_Course")
                .OnDelete(DeleteBehavior.NoAction); ;


            entity.HasOne(d => d.Store).WithMany(p => p.RegisteredCourses)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_RegisteredCourse_Store")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.EnrollCourse).WithMany(p => p.RegisteredCourses)
                .HasForeignKey(d => d.EnrollCourseId)
                .HasConstraintName("FK_RegisteredCourse_EnrollCourse")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Student).WithMany(p => p.RegisteredCourses)
               .HasForeignKey(d => d.StudentId)
               .HasConstraintName("FK_RegisteredCourse_Student")
               .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Review");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Review_Account");

            entity.HasOne(d => d.Store).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Review_Store");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Roles");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.ToTable("Store");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(400);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Descript).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Rated).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StoreImg).HasMaxLength(100);
            entity.Property(e => e.TimeStart).HasPrecision(0);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("Subscription");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Descript).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.NewPrice).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SubTypeId).HasColumnName("SubTypeID");
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.SubType).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.SubTypeId)
                .HasConstraintName("FK_Subscription_SubscriptionType");
        });

        modelBuilder.Entity<SubscriptionType>(entity =>
        {
            entity.ToTable("SubscriptionType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descript).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.ToTable("Unit");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descript).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("Voucher");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.VouCode).HasMaxLength(100);
        });

        modelBuilder.Entity<AccountVoucher>(entity =>
        {
            entity.ToTable("AccountVoucher");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.VoucherID).HasColumnName("VoucherID");
            entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
            entity.Property(e => e.IsAvailable).HasColumnType("bit");

            entity.HasOne(d => d.Voucher).WithMany(p => p.AccountVouchers)
                .HasForeignKey(d => d.VoucherID)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_AccountVouchers_Voucher");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountVouchers)
              .HasForeignKey(d => d.CustomerID)
              .OnDelete(DeleteBehavior.NoAction)
              .HasConstraintName("FK_AccountVouchers_Account");
        });

        modelBuilder.Entity<MentorInfo>(entity =>
        {
            entity.ToTable("MentorInfo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.MentorImg).HasMaxLength(100);
            entity.Property(e => e.PaymentImg).HasMaxLength(100);
            entity.Property(e => e.Salary).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

        });

        modelBuilder.Entity<TableIssues>(entity =>
        {
            entity.ToTable("TableIssues");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BilliardTableID).HasColumnName("BilliardTableID");
            entity.Property(e => e.StoreId).HasColumnName("StoreId");
            entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
            entity.Property(e => e.Descript).HasColumnType("nvarchar(MAX)");
            entity.Property(e => e.ReportedBy).HasMaxLength(500);
            entity.Property(e => e.EstimatedCost).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.RepairStatus).HasMaxLength(50);
            entity.Property(e => e.TableIssuesCode).HasMaxLength(100);
            entity.Property(e => e.IssueImg).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.TableIssues)
               .HasForeignKey(d => d.CustomerID)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_TableIssues_Customer");
            
            entity.HasOne(d => d.Store).WithMany(p => p.TableIssues)
               .HasForeignKey(d => d.StoreId)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_TableIssues_Store");

            entity.HasOne(d => d.BilliardTable).WithMany(p => p.TableIssues)
               .HasForeignKey(d => d.BilliardTableID)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_AccountVouchers_BilliardTable");
        });
        
        modelBuilder.Entity<TableMaintenance>(entity =>
        {
            entity.ToTable("TableMaintenance");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BilliardTableID).HasColumnName("BilliardTableID");
            entity.Property(e => e.StoreId).HasColumnName("StoreId");
            entity.Property(e => e.TechnicianId).HasColumnName("TechnicianId");
            entity.Property(e => e.TableIssuesId).HasColumnName("TableIssuesId");
            entity.Property(e => e.Reason).HasColumnType("nvarchar(MAX)");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.EstimatedCost).HasColumnType("decimal(11, 0)");
            entity.Property(e => e.TableMainCode).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Technician).WithMany(p => p.TableMaintenances)
               .HasForeignKey(d => d.TechnicianId)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_TableMaintenance_Technician");
            
            entity.HasOne(d => d.Store).WithMany(p => p.TableMaintenances)
               .HasForeignKey(d => d.StoreId)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_TableMaintenance_Store");

            entity.HasOne(d => d.BilliardTable).WithMany(p => p.TableMaintenances)
               .HasForeignKey(d => d.BilliardTableID)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_TableMaintenance_BilliardTable");

            entity.HasOne(d => d.TableIssues).WithMany(p => p.TableMaintenances)
             .HasForeignKey(d => d.TableIssuesId)
             .OnDelete(DeleteBehavior.NoAction)
             .HasConstraintName("FK_TableMaintenance_TableIssues");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
            entity.Property(e => e.Descript).HasColumnType("nvarchar(MAX)");
            entity.Property(e => e.IsRead).HasColumnType("BIT").HasDefaultValue(false);
            entity.Property(e => e.ReadAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Notifications)
             .HasForeignKey(d => d.CustomerID)
             .OnDelete(DeleteBehavior.NoAction)
             .HasConstraintName("FK_Notifications_Customer");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
