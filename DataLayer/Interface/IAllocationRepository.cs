using DataLayer.Entities;

namespace DataLayer.Interface;

public interface IAllocationRepository : IGenericRepository<Allocation>
{
    Task<IEnumerable<Allocation>> GetAllocationsByEmployeeIdAsync(int employeeId);
    Task<Allocation> GetAllocationByIdAsync(int id);
    Task<IEnumerable<Allocation>> GetAllocationsByProjectIdAsync(int projectId);
    Task<IEnumerable<Allocation>> GetAllocationsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<bool> SoftDeleteAllocationAsync(int allocationId);
    IQueryable<Allocation> GetAllQueryable();
    Task<IEnumerable<Allocation>> GetAllocationsByLevelAndPositionAsync(int levelId, int positionId);
    Task<IEnumerable<Employee>> GetEmployeesWithAvailableAllocationAsync(int levelId, int positionId, int requiredAllocationPercentage, int projectId);
    Task<int> SharedResource();
    Task<IEnumerable<Allocation>> GetEmployeeAllocationHistory(int employeeId);
    Task<bool> IsProjectManagerAllocatedAsync(int projectId);
    Task<bool> IsTeamLeaderAllocatedAsync(int projectId);


}
