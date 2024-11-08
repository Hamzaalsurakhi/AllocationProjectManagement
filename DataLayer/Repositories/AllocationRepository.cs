using DataLayer.Data;
using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class AllocationRepository : GenericRepository<Allocation>, IAllocationRepository
{
    private readonly AppDbContext _appDbContext;
    public AllocationRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<Allocation>> GetAllocationsByEmployeeIdAsync(int employeeId)
    {
        return await _appDbContext.Allocations
                             .Where(a => a.EmployeeId == employeeId && !a.IsDeleted)
                             .Include(a => a.Project)
                             .Include(a => a.Position)
                             .Include(a => a.Level)
                             .ToListAsync();
    }

    public async Task<Allocation> GetAllocationByIdAsync(int id)
    {
        return await _appDbContext.Allocations
            .Include(a => a.Project)
            .Include(a => a.Employee)
            .Include(a => a.Employee)
            .Include(a => a.Level)
            .Include(a => a.Position)
            .FirstOrDefaultAsync(a => a.AllocationId == id);
    }

    public async Task<IEnumerable<Allocation>> GetAllocationsByProjectIdAsync(int projectId)
    {
        return await _appDbContext.Allocations
            .Where(a => a.projectId == projectId && !a.IsDeleted)
            .Include(a => a.Employee)
            .Include(a => a.Position)
            .Include(a => a.Level)
            .ToListAsync();
    }

    public async Task<IEnumerable<Allocation>> GetAllocationsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _appDbContext.Allocations
            .Where(a => a.StartDate >= startDate && (a.EndDate == null || a.EndDate <= endDate) && !a.IsDeleted)
            .Include(a => a.Employee)
            .Include(a => a.Project)
            .ToListAsync();
    }
    public IQueryable<Allocation> GetAllQueryable()
    {
        return _appDbContext.Allocations
            .Where(a => !a.IsDeleted)
            .Include(a => a.Project)
            .Include(a => a.Employee)
            .Include(a => a.Level)
            .Include(a => a.Position)
            .AsQueryable();
    }


    public async Task<bool> SoftDeleteAllocationAsync(int allocationId)
    {
        var allocation = await _appDbContext.Allocations
            .Include(a => a.Employee)
            .Include(a => a.Employee.ProjectAllocation)
            .FirstOrDefaultAsync(a => a.AllocationId == allocationId);

        if (allocation == null || allocation.IsDeleted)
        {
            return false;
        }

        // Mark the allocation as deleted
        allocation.IsDeleted = true;
        _appDbContext.Allocations.Update(allocation);

        allocation.Employee.TotalAllocatedPercentage -= allocation.AllocationPercentage;

        bool hasActiveAllocations = allocation.Employee.ProjectAllocation
            .Any(a => !a.IsDeleted && (a.EndDate == null || a.EndDate >= DateTime.Now));

        if (!hasActiveAllocations)
        {
            allocation.Employee.IsBench = true;
        }

        _appDbContext.Employees.Update(allocation.Employee);

        await _appDbContext.SaveChangesAsync();
        return true;
    }




    public async Task<IEnumerable<Allocation>> GetAllocationsByLevelAndPositionAsync(int levelId, int positionId)
    {
        return await _appDbContext.Allocations
            .Where(a => a.LevelId == levelId && a.PositionId == positionId && !a.IsDeleted)
            .Include(a => a.Employee)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesWithAvailableAllocationAsync(int levelId, int positionId, int requiredAllocationPercentage, int projectId)
    {
        return await _appDbContext.Employees
             .Where(e => !e.IsDeleted &&
                    e.LevelId == levelId &&
                    e.PositionId == positionId &&
                    !_appDbContext.Allocations.Any(a => a.EmployeeId == e.EmployeeId && a.projectId == projectId && !a.IsDeleted) &&
                    _appDbContext.Allocations.Where(a => a.EmployeeId == e.EmployeeId && !a.IsDeleted)
                    .Sum(a => (int?)a.AllocationPercentage) + requiredAllocationPercentage <= 100)
        .Include(e => e.ProjectAllocation)
        .ToListAsync();
    }

    public async Task<bool> IsProjectManagerAllocatedAsync(int projectId)
    {
        return await _appDbContext.Allocations
            .AnyAsync(a => a.projectId == projectId && a.LevelId == (int)LookUpEnums.levelCategory.ProjectManager && !a.IsDeleted);
    }

    public async Task<bool> IsTeamLeaderAllocatedAsync(int projectId)
    {
        return await _appDbContext.Allocations
            .AnyAsync(a => a.projectId == projectId && a.LevelId == (int)LookUpEnums.levelCategory.TeamLeader && !a.IsDeleted);
    }






















    public async Task<int> SharedResource()
    {
        var allocations = await _appDbContext.Allocations
            .Include(a => a.Employee)
            .Include(a => a.Project)
            .ToListAsync();

        var uniqueEmployeeProjects = allocations.Where(x=>!x.IsDeleted)
            .GroupBy(a => new { a.EmployeeId, a.projectId })
            .Select(g => new
            {
                EmployeeId = g.Key.EmployeeId,
                ProjectId = g.Key.projectId
            })
            .ToList();

        var employeeProjectCounts = uniqueEmployeeProjects
            .GroupBy(up => up.EmployeeId)
            .Select(g => new
            {
                EmployeeId = g.Key,
                ProjectCount = g.Count()
            })
            .ToList();

        int sharedResourceCount = employeeProjectCounts
            .Count(e => e.ProjectCount >= 2);

        return sharedResourceCount;
    }

    public async Task<IEnumerable<Allocation>> GetEmployeeAllocationHistory(int employeeId)
    {
        var empHistory = await _appDbContext.Allocations.Where(e => e.EmployeeId == employeeId)
            .Include(a => a.Employee)
            .Include(e => e.Project)
            .Include(e => e.Level)
            .Include(e => e.Position)
            .OrderBy(ea => ea.StartDate)
            .ToListAsync();
        return empHistory;
    }


}
