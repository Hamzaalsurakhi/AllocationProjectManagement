using BusinessLayer.DTOs;
using BusinessLayer.Helpers;


namespace BusinessLayer.Interfaces
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<DisplayEmployeeDto> Employees, int TotalCount)> GetEmployeesTableAsync(int start, int length, string searchValue, string sortColumn, string sortColumnDirection, string positionId, string levelId);
        Task<(IEnumerable<DisplayEmployeeDto> Employees, int TotalCount)> GetIsBenchEmployeesTableAsync(int start, int length, string searchValue, string sortColumn, string sortColumnDirection, string positionId, string levelId);
        Task<EmployeeDto> GetEmployeeByEmailAsync(string email);
        Task<IEnumerable<DisplayEmployeeDto>> GetAllEmployeesAsync();
        Task<DisplayEmployeeDto> GetEmployeeByIdAsync( int id );
        //Task<EmployeeDto> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(EmployeeDto employeeDto);
        Task AddEmployeesAsync(List<EmployeeDto> employeeDtos);
        Task UpdateEmployeeAsync(EmployeeDto employeeDto);
        Task<bool> SoftDeleteEmployeeAsync(int id);
        Task<int> NumberOfEmployeeInBench();
        Task<int> NumberOfSharedResource();
        Task<IQueryable<DisplayEmployeeDto>> GetAllEmployeeIsBench();

        Task<List<DisplayEmployeeDto>> GetAllEmployeeFilteredReport(DateTime startDate, DateTime endDate, string search, int page, int pageSize);
        Task<(IEnumerable<DisplayEmployeeDto> Employees, int TotalCount)> GetDeletedEmployeesTableAsync(int start, int length, string searchValue, string sortColumn, string sortColumnDirection);
        Task<bool> RestoreEmployeeAsync(int id);
        Task<IEnumerable<EmployeeWithTimeDifferenceDto>> EmployeesBecomeBench();
    }
     
}

