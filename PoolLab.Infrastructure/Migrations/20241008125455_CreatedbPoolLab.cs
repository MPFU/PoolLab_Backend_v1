using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatedbPoolLab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BilliardPrice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    OldPrice = table.Column<decimal>(type: "decimal(11,0)", nullable: true),
                    NewPrice = table.Column<decimal>(type: "decimal(11,0)", nullable: true),
                    TimeStart = table.Column<DateTime>(type: "datetime", nullable: true),
                    TimeEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BilliardPrice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BilliardType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BilliardType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CompanyImg = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MentorInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MentorImg = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(11,0)", nullable: true),
                    PaymentImg = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Point = table.Column<int>(type: "int", nullable: true),
                    VouCode = table.Column<int>(type: "int", nullable: true),
                    TypeCode = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BilliardTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AreaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BilliardTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Qrcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PriceID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BilliardTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BilliardTable_Area",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BilliardTable_BilliardPrice",
                        column: x => x.PriceID,
                        principalTable: "BilliardPrice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BilliardTable_BilliardType",
                        column: x => x.BilliardTypeID,
                        principalTable: "BilliardType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    StoreImg = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Rated = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    TimeStart = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    TimeEnd = table.Column<TimeOnly>(type: "time", nullable: true),
                    CompanyID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Store_Company",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(11,0)", nullable: true),
                    SubTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TimeStart = table.Column<DateTime>(type: "datetime", nullable: true),
                    TimeEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_SubscriptionType",
                        column: x => x.SubTypeID,
                        principalTable: "SubscriptionType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    MinQuantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(11,0)", nullable: true),
                    ProductImg = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductGroupID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_GroupProduct",
                        column: x => x.ProductGroupID,
                        principalTable: "GroupProduct",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Product_ProductType",
                        column: x => x.ProductTypeID,
                        principalTable: "ProductType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Product_Unit",
                        column: x => x.UnitID,
                        principalTable: "Unit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(11,0)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Course_MentorInfo",
                        column: x => x.MentorId,
                        principalTable: "MentorInfo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Course_Store",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Point = table.Column<int>(type: "int", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(11,0)", nullable: true),
                    TotalTime = table.Column<int>(type: "int", nullable: true),
                    Rank = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tier = table.Column<int>(type: "int", nullable: true),
                    SubID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Company",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_Store",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Account_Subscription",
                        column: x => x.SubID,
                        principalTable: "Subscription",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RegisteredCourse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisteredCourse_Course",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegisteredCourse_Store",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountVoucher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VoucherID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountVoucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountVouchers_Account",
                        column: x => x.CustomerID,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountVouchers_Voucher",
                        column: x => x.VoucherID,
                        principalTable: "Voucher",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BilliardTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BilliardTableID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TimeStart = table.Column<DateTime>(type: "datetime", nullable: true),
                    TimeEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_Account",
                        column: x => x.CustomerID,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_BilliardTable",
                        column: x => x.BilliardTableID,
                        principalTable: "BilliardTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_BilliardType",
                        column: x => x.BilliardTypeID,
                        principalTable: "BilliardType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_Store",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TimeStart = table.Column<DateTime>(type: "datetime", nullable: true),
                    TimeEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_Account",
                        column: x => x.ManagerID,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Event_Store",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(11,1)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(11,0)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Account",
                        column: x => x.CustomerID,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecurringBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BilliardTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BilliardTableID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecBook_Account",
                        column: x => x.CustomerID,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecBook_BilliardTable",
                        column: x => x.BilliardTableID,
                        principalTable: "BilliardTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecBook_BilliardType",
                        column: x => x.BilliardTypeID,
                        principalTable: "BilliardType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecBook_Store",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rated = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_Account",
                        column: x => x.CustomerID,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Review_Store",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BilliardTableID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(11,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_BilliardTable",
                        column: x => x.BilliardTableID,
                        principalTable: "BilliardTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDetail_Product",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(11,0)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentInfo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PaymentCode = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Account",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payment_Order",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payment_Subscription",
                        column: x => x.SubID,
                        principalTable: "Subscription",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayTime",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BilliardTableID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TimeStart = table.Column<DateTime>(type: "datetime", nullable: true),
                    TimeEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    TotalTime = table.Column<decimal>(type: "decimal(11,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(11,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayTime_BilliardTable",
                        column: x => x.BilliardTableID,
                        principalTable: "BilliardTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayTime_Order",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TableAvailability",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BilliardTableID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecurringBookingID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableAvailability_BilliardTable",
                        column: x => x.BilliardTableID,
                        principalTable: "BilliardTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TableAvailability_Booking",
                        column: x => x.BookingID,
                        principalTable: "Booking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TableAvailability_RecurringBooking",
                        column: x => x.RecurringBookingID,
                        principalTable: "RecurringBookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CompanyID",
                table: "Account",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_StoreId",
                table: "Account",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_SubID",
                table: "Account",
                column: "SubID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountVoucher_CustomerID",
                table: "AccountVoucher",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountVoucher_VoucherID",
                table: "AccountVoucher",
                column: "VoucherID");

            migrationBuilder.CreateIndex(
                name: "IX_BilliardTable_AreaID",
                table: "BilliardTable",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_BilliardTable_BilliardTypeID",
                table: "BilliardTable",
                column: "BilliardTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_BilliardTable_PriceID",
                table: "BilliardTable",
                column: "PriceID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_BilliardTableID",
                table: "Booking",
                column: "BilliardTableID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_BilliardTypeID",
                table: "Booking",
                column: "BilliardTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerID",
                table: "Booking",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_StoreID",
                table: "Booking",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Course_MentorId",
                table: "Course",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_StoreId",
                table: "Course",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ManagerID",
                table: "Event",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_StoreID",
                table: "Event",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerID",
                table: "Order",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_BilliardTableID",
                table: "OrderDetail",
                column: "BilliardTableID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderID",
                table: "OrderDetail",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductID",
                table: "OrderDetail",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AccountID",
                table: "Payment",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderID",
                table: "Payment",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_SubID",
                table: "Payment",
                column: "SubID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayTime_BilliardTableID",
                table: "PlayTime",
                column: "BilliardTableID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayTime_OrderID",
                table: "PlayTime",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductGroupID",
                table: "Product",
                column: "ProductGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductTypeID",
                table: "Product",
                column: "ProductTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_UnitID",
                table: "Product",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringBookings_BilliardTableID",
                table: "RecurringBookings",
                column: "BilliardTableID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringBookings_BilliardTypeID",
                table: "RecurringBookings",
                column: "BilliardTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringBookings_CustomerID",
                table: "RecurringBookings",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringBookings_StoreID",
                table: "RecurringBookings",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCourse_CourseID",
                table: "RegisteredCourse",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCourse_StoreID",
                table: "RegisteredCourse",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_CustomerID",
                table: "Review",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_StoreID",
                table: "Review",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Store_CompanyID",
                table: "Store",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SubTypeID",
                table: "Subscription",
                column: "SubTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TableAvailability_BilliardTableID",
                table: "TableAvailability",
                column: "BilliardTableID");

            migrationBuilder.CreateIndex(
                name: "IX_TableAvailability_BookingID",
                table: "TableAvailability",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_TableAvailability_RecurringBookingID",
                table: "TableAvailability",
                column: "RecurringBookingID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountVoucher");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "PlayTime");

            migrationBuilder.DropTable(
                name: "RegisteredCourse");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "TableAvailability");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "RecurringBookings");

            migrationBuilder.DropTable(
                name: "GroupProduct");

            migrationBuilder.DropTable(
                name: "ProductType");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "MentorInfo");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "BilliardTable");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "BilliardPrice");

            migrationBuilder.DropTable(
                name: "BilliardType");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "SubscriptionType");
        }
    }
}
