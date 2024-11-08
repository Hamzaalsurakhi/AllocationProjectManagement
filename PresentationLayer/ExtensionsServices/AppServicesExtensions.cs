using BusinessLayer.Infastructure;
using DataLayer.Data;
using DataLayer.Infastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.ViewModels;


namespace PresentationLayer.ExtensionsServices
{
    public static class AppServicesExtensions
    {
        public static IServiceCollection AddServiceExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));

            });
            services.AddFluentValidationAutoValidation();
            services.AddServiceRepository();//service form Data
            services.AddBusinessServiceModule(); //service form Business
            services.AddTransient<IValidator<EmployeeViewModel>, EmployeesValidator>();
            services.AddTransient<IValidator<ProjectViewModel>, ProjectValidator>();
            services.AddTransient<IValidator<CreateAllocationViewModel>, CreateAllocationValidator>();
            services.AddTransient<IValidator<EditAllocationViewModel>, EditAllocationValidator>();
            services.AddHttpContextAccessor();
            services.AddCors();
            return services;
        }
    }
}
