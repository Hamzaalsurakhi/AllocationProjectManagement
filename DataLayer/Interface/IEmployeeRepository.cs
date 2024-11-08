using DataLayer.Entities;

namespace DataLayer.Interface
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task<bool> SoftDeleteEmployeeAsync(int employeeId);
        IQueryable<Employee> GetAllQueryable();

        IQueryable<Employee> GetDeletedQueryable();
        Task<IEnumerable<Employee>> EmployeesBecomeBench();
        Task<Employee> GetEmployeeByEmailAsync(string email);

    }
}
