using BusinessLayer.DTOs;
using DataLayer.Entities;

namespace BusinessLayer.Interfaces;

public interface IAllocationService
{
    Task<IEnumerable<AllocationDto>> CreateAllocationAsync(AddAllocationDto allocationDto);
    Task<IEnumerable<AllocationDto>> GetAllAllocationsAsync();
    Task<IEnumerable<AllocationDto>> GetAllocationsByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<AllocationDto>> GetAllocationsByProjectIdAsync(int projectId);


    Task<bool> SoftDeleteAllocationAsync(int id);
    Task<(List<AllocationDto> Allocations, int TotalCount)> GetAllocationTableAsync(int start, int length, string searchValue, string sortColumn, string sortColumnDirection);
    Task<IEnumerable<EmployeeAllocationDto>> GetEmployeesForAllocationAsync(int levelId, int positionId, int allocationPercentage, int projectId);
    /// <summary>
    Task<int> TotalOfSharedResource();
    Task<Dictionary<string, Dictionary<string, int>>> GetEmployeeDistributionByProjectAsync();
    Task<List<AllocationReportDto>> GetAllocationsFilteredReport(DateTime startDate, DateTime endDate, string search, int page, int pageSize);
    Task<IEnumerable<AllocationHistoryDto>> GetEmployeeAllocationHistoryAsync(int employeeId, string searchValue, string sortColumn, string sortColumnDirection);

    Task<AllocationDto> GetAllocationByIdAsync(int id);


    Task<bool> IsProjectManagerAllocatedAsync(int projectId);
    Task<bool> IsTeamLeaderAllocatedAsync(int projectId);


    Task<AllocationDto> EditAllocationAsync(EditAllocationDto allocationDto);
    Task UpdateEmployeesStatusAsync();
    Task<IEnumerable<Project>> GetFilteredProjectsAsync(int employeeId, int currentAllocationId);
}
