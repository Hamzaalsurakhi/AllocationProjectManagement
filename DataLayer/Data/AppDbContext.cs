using DataLayer.Entities;
using DataLayer.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DataLayer.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var userClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;

                    if (int.TryParse(userClaim, out int userId))
                    {
                        entry.Entity.CreatedBy = userId;
                    }
                    else
                    {
                        throw new InvalidOperationException("User ID claim is missing or invalid.");
                    }

                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifyOn = DateTime.UtcNow;
                    if (int.TryParse(userClaim, out int userId))
                    {
                        entry.Entity.ModifyBy = userId;
                    }
                    else
                    {
                        throw new InvalidOperationException("User ID claim is missing or invalid.");
                    }
                }
            }

            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var userClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;

                    if (int.TryParse(userClaim, out int userId))
                    {
                        entry.Entity.CreatedBy = userId;
                    }
                    else
                    {
                        throw new InvalidOperationException("User ID claim is missing or invalid.");
                    }

                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifyOn = DateTime.UtcNow;
                    if (int.TryParse(userClaim, out int userId))
                    {
                        entry.Entity.ModifyBy = userId;
                    }
                    else
                    {
                        throw new InvalidOperationException("User ID claim is missing or invalid.");
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LookUp>().ToTable("LookUp", "LookUp");
            builder.Entity<LookUpCategory>().ToTable("LookUpCategory", "LookUp");
            builder.Entity<Settings>().ToTable("Settings", "Settings");

            builder.Entity<Allocation>()
                .HasOne(a => a.Project)
                .WithMany(p => p.allocations)
                .HasForeignKey(a => a.projectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Allocation>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.ProjectAllocation)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Employee>()
                .HasOne(e => e.Position)
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Employee>()
                .HasOne(e => e.Level)
                .WithMany()
                .HasForeignKey(e => e.LevelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LookUp>()
                .HasOne(l => l.LookupCategory)
                .WithMany(c => c.lookUps)
                .HasForeignKey(l => l.LookupCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                .HasOne(p => p.Status)
                .WithMany()
                .HasForeignKey(p => p.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
            // Seed data for LookUpCategory
            builder.Entity<LookUpCategory>().HasData(
                new LookUpCategory
                {
                    CategoryId = (int)LookUpEnums.CategoryCode.Position,
                    NameEn = "Position",
                    NameAr = "الموقع",
                    Description = "Job Positions",
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUpCategory
                {
                    CategoryId = (int)LookUpEnums.CategoryCode.Level,
                    NameEn = "Level",
                    NameAr = "المستوى",
                    Description = "Experience Levels",
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUpCategory
                {
                    CategoryId = (int)LookUpEnums.CategoryCode.Status,
                    NameEn = "Status",
                    NameAr = "الحالة",
                    Description = "Project Statuses",
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                 new LookUpCategory
                 {
                     CategoryId = (int)LookUpEnums.CategoryCode.TeamCountry,
                     NameEn = "TeamCountry",
                     NameAr = "دولة الفريق",
                     Description = "Team in which country is",
                     CreatedBy = 1,
                     IsDeleted = false,
                     CreatedOn = DateTime.Now,
                     ModifyBy = null,
                     ModifyOn = null,
                 }

            );

            // Seed data for LookUp
            builder.Entity<LookUp>().HasData(
                // Position lookups
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.PositionCategory.FullStack,
                    NameEn = "Full Stack Developer",
                    NameAr = "مطور متكامل",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Position,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.PositionCategory.Frontend,
                    NameEn = "Frontend Developer",
                    NameAr = "مطور الواجهة الأمامية",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Position,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.PositionCategory.Backend,
                    NameEn = "Backend Developer",
                    NameAr = "مطور الواجهة الخلفية",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Position,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.PositionCategory.AI,
                    NameEn = "AI Specialist",
                    NameAr = "متخصص في الذكاء الاصطناعي",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Position,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.PositionCategory.UX,
                    NameEn = "UX Designer",
                    NameAr = "مصمم تجربة المستخدم",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Position,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.PositionCategory.QA,
                    NameEn = "QA Tester",
                    NameAr = "مختبر ضمان الجودة",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Position,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.PositionCategory.BusinessAnalyst,
                    NameEn = "Business Analyst",
                    NameAr = "محلل أعمال",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Position,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.PositionCategory.AssistantManager,
                    NameEn = "Assistant Manager",
                    NameAr = "مساعد المدير",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Position,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                // Level lookups
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.levelCategory.Fresh,
                    NameEn = "Fresh",
                    NameAr = "جديد",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Level,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.levelCategory.Junior,
                    NameEn = "Junior",
                    NameAr = "مبتدئ",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Level,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.levelCategory.MidSeniorLevel,
                    NameEn = "Mid-Senior Level",
                    NameAr = "مستوى متوسط - كبير",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Level,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.levelCategory.Senior,
                    NameEn = "Senior",
                    NameAr = "كبير",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Level,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.levelCategory.TeamLeader,
                    NameEn = "Team Leader",
                    NameAr = "قائد الفريق",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Level,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.levelCategory.ProjectManager,
                    NameEn = "Project Manager",
                    NameAr = " مدير المشروع",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Level,
                    CreatedBy = 2,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                // Status lookups
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.StatusCategory.Active,
                    NameEn = "Active",
                    NameAr = "نشط",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Status,
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.StatusCategory.OnHold,
                    NameEn = "On Hold",
                    NameAr = "معلق",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Status,
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.StatusCategory.Finished,
                    NameEn = "Finished",
                    NameAr = "منتهي",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Status,
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,

                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.StatusCategory.Design,
                    NameEn = "Design",
                    NameAr = "التصميم",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Status,
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,

                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.StatusCategory.Development,
                    NameEn = "Development",
                    NameAr = "التطوير",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Status,
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,

                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.StatusCategory.Deployment,
                    NameEn = "Deployment",
                    NameAr = "نشر",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Status,
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,

                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.StatusCategory.Testing,
                    NameEn = "Testing",
                    NameAr = "الفحص",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.Status,
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,

                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.TeamCountryCategory.TeamJordan,
                    NameEn = "Jordan",
                    NameAr = "الاردن",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.TeamCountry,
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,

                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.TeamCountryCategory.TeamSaudiArabia,
                    NameEn = "Saudi Arabia",
                    NameAr = "المملكة العربية السعودية",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.TeamCountry,
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,

                },
                new LookUp
                {
                    LookUpId = (int)LookUpEnums.TeamCountryCategory.TeamEgypt,
                    NameEn = "Egypt",
                    NameAr = "مصر",
                    LookupCategoryId = (int)LookUpEnums.CategoryCode.TeamCountry,
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,

                }

            );
            // Seed data for Settings
            builder.Entity<Settings>().HasData(
                new Settings
                {
                    SettingId = 1,
                    Key = (int)SettingsEnum.SettingEnum.AllowedFileTypes,
                    Value = "jpg,png,pdf",
                    Description = "Allowed types of files that can be uploaded",
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new Settings
                {
                    SettingId = 2,
                    Key = (int)SettingsEnum.SettingEnum.ApplicationName,
                    Value = "Allocation Management System,AMS",
                    Description = "Name of the application",
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                 new Settings
                 {
                     SettingId = 3,
                     Key = (int)SettingsEnum.SettingEnum.DefaultLanguage,
                     Value = "en-US",
                     Description = "Default language for the application",
                     CreatedBy = 1,
                     IsDeleted = false,
                     CreatedOn = DateTime.Now,
                     ModifyBy = null,
                     ModifyOn = null,
                 },
                new Settings
                {
                    SettingId = 4,
                    Key = (int)SettingsEnum.SettingEnum.DefaultTimeZone,
                    Value = "UTC",
                    Description = "Default timezone",
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new Settings
                {
                    SettingId = 5,
                    Key = (int)SettingsEnum.SettingEnum.DateFormat,
                    Value = "MM/dd/yyyy,dd/MM/yyyy",
                    Description = "Date format (e.g., MM/dd/yyyy or dd/MM/yyyy)",
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new Settings
                {
                    SettingId = 6,
                    Key = (int)SettingsEnum.SettingEnum.TimeFormat,
                    Value = "12-hour,24-hour",
                    Description = "Time format (e.g., 12-hour or 24-hour)",
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new Settings
                {
                    SettingId = 7,
                    Key = (int)SettingsEnum.SettingEnum.MaxLoginAttempts,
                    Value = "5",
                    Description = "Maximum number of failed login attempts before locking an account",
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new Settings
                {
                    SettingId = 8,
                    Key = (int)SettingsEnum.SettingEnum.DefaultReportFormat,
                    Value = "PDF,Excel",
                    Description = "Default report format (PDF, Excel)",
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new Settings
                {
                    SettingId = 9,
                    Key = (int)SettingsEnum.SettingEnum.MaxFileSize,
                    Value = "10",
                    Description = "Maximum allowed file size in megabytes",
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                new Settings
                {
                    SettingId = 10,
                    Key = (int)SettingsEnum.SettingEnum.SiteUrl,
                    Value = "",
                    Description = "The URL of the site",
                    CreatedBy = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifyBy = null,
                    ModifyOn = null,
                },
                 new Settings
                 {
                     SettingId = 11,
                     Key = (int)SettingsEnum.SettingEnum.NumberOfEmployeeBecomeBench,
                     Value = "6",
                     Description = "Number Of Employees Become Bench",
                     CreatedBy = 1,
                     IsDeleted = false,
                     CreatedOn = DateTime.Now,
                     ModifyBy = null,
                     ModifyOn = null,
                 },
                        new Settings
                        {
                            SettingId = 12,
                            Key = (int)SettingsEnum.SettingEnum.TypeOfFile,
                            Value = "png,jpg,jpeg",
                            Description = "Type Of File",
                            CreatedBy = 1,
                            IsDeleted = false,
                            CreatedOn = DateTime.Now,
                            ModifyBy = null,
                            ModifyOn = null,
                        },

                            new Settings
                            {
                                SettingId = 13,
                                Key = (int)SettingsEnum.SettingEnum.NumberOfNotificationsDisplay,
                                Value = "5",
                                Description = "Number Of Notifications Display",
                                CreatedBy = 1,
                                IsDeleted = false,
                                CreatedOn = DateTime.Now,
                                ModifyBy = null,
                                ModifyOn = null,
                            }

               );
        }
        public DbSet<Project> projects { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Allocation> Allocations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LookUp> LookUps { get; set; }
        public DbSet<LookUpCategory> LookUpCategories { get; set; }
        public DbSet<Settings> Settings { get; set; }

    }
}
