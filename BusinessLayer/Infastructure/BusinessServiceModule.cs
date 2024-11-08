using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using BusinessLayer.Repositories;
using BusinessLayer.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace BusinessLayer.Infastructure
{
    public static class BusinessServiceModule
    {
        public static void AddBusinessServiceModule(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ILookUpService, LookUpService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAllocationService, AllocationService>();
            services.AddTransient<IValidator<AddAllocationDto>, AddAllocationDtoValidator>();
            services.AddTransient<IValidator<EditAllocationDto>, EditAllocationDtoValidator>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
