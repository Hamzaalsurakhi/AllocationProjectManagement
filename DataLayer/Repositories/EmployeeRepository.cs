using DataLayer.Data;
using DataLayer.Entities;
using DataLayer.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;
        public EmployeeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _appDbContext.Employees.Where(e => !e.IsDeleted).Include(e => e.Level).Include(e => e.Position).Include(e => e.TeamCountry).ToListAsync();
        }

        public IQueryable<Employee> GetAllQueryable()
        {
            return _appDbContext.Employees.Where(e => !e.IsDeleted).Include(e => e.Level).Include(e => e.Position).Include(e => e.TeamCountry).AsQueryable();
        }
       

        public async Task<bool> SoftDeleteEmployeeAsync(int employeeId)
        {
            var employee = await _appDbContext.Employees
              .FindAsync(employeeId);

            if (employee == null)
            {
                return false;
            }

            if (employee.IsBench == false)
            {
                throw new InvalidOperationException("Employee has active allocations.");
            }

            employee.IsDeleted = true;
            _appDbContext.Employees.Update(employee);
            return true;
        }


        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _appDbContext.Employees
                .Include(e => e.Level)
                .Include(e => e.Position)
                .Include(e => e.TeamCountry)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }
        public IQueryable<Employee> GetDeletedQueryable()
        {
            return _appDbContext.Employees.Where(e => e.IsDeleted).Include(e => e.Level).Include(e => e.Position).Include(e => e.TeamCountry).AsQueryable();
        }

        public async Task<IEnumerable<Employee>> EmployeesBecomeBench()
        {
            var employees = await _appDbContext.Employees.Where(e => !e.IsDeleted).ToListAsync();
            var allocations = await _appDbContext.Allocations.Where(e => !e.IsDeleted).Include(x => x.Employee).Include(x => x.Project).Include(x => x.Position).Include(x => x.Level).ToListAsync();
            var projects = await _appDbContext.Projects.Where(e => !e.IsDeleted).ToListAsync();

            var employeeAllocations = allocations
                .Join(projects,
                      a => a.projectId,
                      p => p.ProjectId,
                      (a, p) => new { a.EmployeeId, a.EndDate, ProjectEndDate = p.EndDate })
                .Where(ap => ap.EndDate <= ap.ProjectEndDate)
                .GroupBy(ap => ap.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    LatestEndDate = g.Max(ap => ap.EndDate)
                })
                .ToList();

            var employeesWithTimeDifference = employeeAllocations
                .Select(ea => new
                {
                    EmployeeId = ea.EmployeeId,
                    TimeDifference = (ea.LatestEndDate - DateTime.Now)
                })
                .Where(td => (td.TimeDifference.Value.TotalDays <= 3 && td.TimeDifference.Value.TotalDays > 0) ||
                             (td.TimeDifference.Value.TotalHours < 24 && td.TimeDifference.Value.TotalDays <= 0))
                .OrderBy(td => td.TimeDifference)
                .Take(6)
                .ToList();

            var topEmployees = employeesWithTimeDifference
                .Join(employees,
                      td => td.EmployeeId,
                      e => e.EmployeeId,
                      (td, e) => e)
                .ToList();
            return topEmployees;
        }

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            return await _appDbContext.Employees.FirstOrDefaultAsync(x => x.Email == email);
        }
    }



}
