using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LookUp");

            migrationBuilder.EnsureSchema(
                name: "Settings");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LookUpCategory",
                schema: "LookUp",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookUpCategory", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                schema: "Settings",
                columns: table => new
                {
                    SettingId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LookUp",
                schema: "LookUp",
                columns: table => new
                {
                    LookUpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LookupCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookUp", x => x.LookUpId);
                    table.ForeignKey(
                        name: "FK_LookUp_LookUpCategory_LookupCategoryId",
                        column: x => x.LookupCategoryId,
                        principalSchema: "LookUp",
                        principalTable: "LookUpCategory",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MidName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FourthName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsBench = table.Column<bool>(type: "bit", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    TotalAllocatedPercentage = table.Column<int>(type: "int", nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    TeamCountryId = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_LookUp_LevelId",
                        column: x => x.LevelId,
                        principalSchema: "LookUp",
                        principalTable: "LookUp",
                        principalColumn: "LookUpId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_LookUp_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "LookUp",
                        principalTable: "LookUp",
                        principalColumn: "LookUpId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_LookUp_TeamCountryId",
                        column: x => x.TeamCountryId,
                        principalSchema: "LookUp",
                        principalTable: "LookUp",
                        principalColumn: "LookUpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Project_LookUp_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "LookUp",
                        principalTable: "LookUp",
                        principalColumn: "LookUpId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Allocations",
                columns: table => new
                {
                    AllocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    projectId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AllocationPercentage = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LevelId = table.Column<int>(type: "int", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocations", x => x.AllocationId);
                    table.ForeignKey(
                        name: "FK_Allocations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Allocations_LookUp_LevelId",
                        column: x => x.LevelId,
                        principalSchema: "LookUp",
                        principalTable: "LookUp",
                        principalColumn: "LookUpId");
                    table.ForeignKey(
                        name: "FK_Allocations_LookUp_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "LookUp",
                        principalTable: "LookUp",
                        principalColumn: "LookUpId");
                    table.ForeignKey(
                        name: "FK_Allocations_Project_projectId",
                        column: x => x.projectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "LookUp",
                table: "LookUpCategory",
                columns: new[] { "CategoryId", "CreatedBy", "CreatedOn", "Description", "IsDeleted", "ModifyBy", "ModifyOn", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2746), "Job Positions", false, null, null, "الموقع", "Position" },
                    { 2, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2761), "Experience Levels", false, null, null, "المستوى", "Level" },
                    { 3, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2764), "Project Statuses", false, null, null, "الحالة", "Status" },
                    { 4, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2767), "Team in which country is", false, null, null, "دولة الفريق", "TeamCountry" }
                });

            migrationBuilder.InsertData(
                schema: "Settings",
                table: "Settings",
                columns: new[] { "SettingId", "CreatedBy", "CreatedOn", "Description", "IsDeleted", "Key", "ModifyBy", "ModifyOn", "Value" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2985), "Allowed types of files that can be uploaded", false, 8, null, null, "jpg,png,pdf" },
                    { 2, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2988), "Name of the application", false, 1, null, null, "Allocation Management System,AMS" },
                    { 3, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2990), "Default language for the application", false, 2, null, null, "en-US" },
                    { 4, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2992), "Default timezone", false, 3, null, null, "UTC" },
                    { 5, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2994), "Date format (e.g., MM/dd/yyyy or dd/MM/yyyy)", false, 4, null, null, "MM/dd/yyyy,dd/MM/yyyy" },
                    { 6, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2996), "Time format (e.g., 12-hour or 24-hour)", false, 5, null, null, "12-hour,24-hour" },
                    { 7, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2999), "Maximum number of failed login attempts before locking an account", false, 6, null, null, "5" },
                    { 8, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(3001), "Default report format (PDF, Excel)", false, 7, null, null, "PDF,Excel" },
                    { 9, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(3003), "Maximum allowed file size in megabytes", false, 9, null, null, "10" },
                    { 10, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(3005), "The URL of the site", false, 10, null, null, "" },
                    { 11, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(3007), "Number Of Employees Become Bench", false, 11, null, null, "6" },
                    { 12, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(3009), "Type Of File", false, 12, null, null, "png,jpg,jpeg" },
                    { 13, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(3011), "Number Of Notifications Display", false, 13, null, null, "5" }
                });

            migrationBuilder.InsertData(
                schema: "LookUp",
                table: "LookUp",
                columns: new[] { "LookUpId", "CreatedBy", "CreatedOn", "IsDeleted", "LookupCategoryId", "ModifyBy", "ModifyOn", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 10, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2913), false, 1, null, null, "مطور متكامل", "Full Stack Developer" },
                    { 11, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2917), false, 1, null, null, "مطور الواجهة الأمامية", "Frontend Developer" },
                    { 12, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2919), false, 1, null, null, "مطور الواجهة الخلفية", "Backend Developer" },
                    { 13, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2921), false, 1, null, null, "متخصص في الذكاء الاصطناعي", "AI Specialist" },
                    { 14, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2923), false, 1, null, null, "مصمم تجربة المستخدم", "UX Designer" },
                    { 15, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2925), false, 1, null, null, "مختبر ضمان الجودة", "QA Tester" },
                    { 16, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2927), false, 1, null, null, "محلل أعمال", "Business Analyst" },
                    { 17, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2929), false, 1, null, null, "مساعد المدير", "Assistant Manager" },
                    { 31, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2931), false, 2, null, null, "جديد", "Fresh" },
                    { 32, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2933), false, 2, null, null, "مبتدئ", "Junior" },
                    { 33, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2935), false, 2, null, null, "مستوى متوسط - كبير", "Mid-Senior Level" },
                    { 34, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2937), false, 2, null, null, "كبير", "Senior" },
                    { 35, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2939), false, 2, null, null, "قائد الفريق", "Team Leader" },
                    { 36, 2, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2941), false, 2, null, null, " مدير المشروع", "Project Manager" },
                    { 41, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2943), false, 3, null, null, "نشط", "Active" },
                    { 42, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2945), false, 3, null, null, "معلق", "On Hold" },
                    { 43, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2949), false, 3, null, null, "التصميم", "Design" },
                    { 45, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2953), false, 3, null, null, "نشر", "Deployment" },
                    { 46, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2955), false, 3, null, null, "الفحص", "Testing" },
                    { 47, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2951), false, 3, null, null, "التطوير", "Development" },
                    { 48, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2947), false, 3, null, null, "منتهي", "Finished" },
                    { 51, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2957), false, 4, null, null, "الاردن", "Jordan" },
                    { 52, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2960), false, 4, null, null, "المملكة العربية السعودية", "Saudi Arabia" },
                    { 53, 1, new DateTime(2024, 10, 2, 18, 46, 9, 803, DateTimeKind.Local).AddTicks(2962), false, 4, null, null, "مصر", "Egypt" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_EmployeeId",
                table: "Allocations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_LevelId",
                table: "Allocations",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_PositionId",
                table: "Allocations",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_projectId",
                table: "Allocations",
                column: "projectId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LevelId",
                table: "Employees",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionId",
                table: "Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TeamCountryId",
                table: "Employees",
                column: "TeamCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_LookUp_LookupCategoryId",
                schema: "LookUp",
                table: "LookUp",
                column: "LookupCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_StatusId",
                table: "Project",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allocations");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "Settings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LookUp",
                schema: "LookUp");

            migrationBuilder.DropTable(
                name: "LookUpCategory",
                schema: "LookUp");
        }
    }
}
