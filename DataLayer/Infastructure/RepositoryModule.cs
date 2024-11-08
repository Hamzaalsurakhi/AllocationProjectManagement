using DataLayer.Interface;
using DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Infastructure
{
    public static class RepositoryModule
    {
        public static void AddServiceRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ILookUpRepository, LookUpRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IAllocationRepository, AllocationRepository>();
        }
    }
}
