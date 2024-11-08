using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataLayer.Entities;
using DataLayer.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace BusinessLayer.Services;

public class AllocationService : IAllocationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    public AllocationService(IUnitOfWork unitOfWork, IMapper mapper,INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _notificationService = notificationService;
        }

    public async Task<IEnumerable<AllocationDto>> CreateAllocationAsync(AddAllocationDto allocationDto)
    {

        var validatore = new AddAllocationDtoValidator(_unitOfWork);

        var validationResult = await validatore.ValidateAsync(allocationDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var newAllocations = new List<Allocation>();
        
        foreach (var employeeId in allocationDto.SelectedEmployeeIds)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);

            var allocation = _mapper.Map<Allocation>(allocationDto);
            allocation.EmployeeId = employeeId;
            newAllocations.Add(allocation);

            employee.IsBench = false;
            employee.TotalAllocatedPercentage = employee.TotalAllocatedPercentage + allocationDto.AllocationPercentage;
            _unitOfWork.EmployeeRepository.Update(employee);
        }
        await _unitOfWork.AllocationRepository.AddRangeAsync(newAllocations);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<IEnumerable<AllocationDto>>(newAllocations);
    }


    public async Task UpdateEmployeesStatusAsync()
    {
        var now = DateTime.Now;

        var endedAllocations = await _unitOfWork.AllocationRepository
            .GetAllQueryable()
            .Where(a => a.EndDate.HasValue && a.EndDate <= now && !a.IsDeleted)
            .Include(a => a.Employee)
            .ToListAsync();

        foreach (var allocation in endedAllocations)
        {
            var employee = allocation.Employee;

            employee.TotalAllocatedPercentage -= allocation.AllocationPercentage;

            var hasActiveAllocations = await _unitOfWork.AllocationRepository
                .GetAllQueryable()
                .AnyAsync(a => a.EmployeeId == employee.EmployeeId && a.EndDate > now && !a.IsDeleted);

            employee.IsBench = !hasActiveAllocations;
            if(employee.TotalAllocatedPercentage==0&&employee.IsBench==true)
                {
                await _notificationService.CreateNotificationAsync("Bench Status",$"Employee {employee.FirstName} {employee.MidName} {employee.LastName} is on the bench.");
                }
            allocation.IsDeleted = true;

            _unitOfWork.EmployeeRepository.Update(employee);
            _unitOfWork.AllocationRepository.Update(allocation);
        }

        await _unitOfWork.CompleteAsync();
    }
   
    public async Task<AllocationDto> GetAllocationByIdAsync(int id)
    {
        var allocation = await _unitOfWork.AllocationRepository.GetAllocationByIdAsync(id);
        if (allocation == null)
        {
            return null;
        }

        return _mapper.Map<AllocationDto>(allocation);
    }



   

    public async Task<bool> IsProjectManagerAllocatedAsync(int projectId)
    {
        return await _unitOfWork.AllocationRepository.IsProjectManagerAllocatedAsync(projectId);
    }

    public async Task<bool> IsTeamLeaderAllocatedAsync(int projectId)
    {
        return await _unitOfWork.AllocationRepository.IsTeamLeaderAllocatedAsync(projectId);
    }


    public async Task<IEnumerable<AllocationDto>> GetAllAllocationsAsync()
    {
        var allocations = await _unitOfWork.AllocationRepository.GetAllQueryable().ToListAsync();
        return _mapper.Map<IEnumerable<AllocationDto>>(allocations);
    }

    public async Task<IEnumerable<AllocationDto>> GetAllocationsByEmployeeIdAsync(int employeeId)
    {
        var allocations = await _unitOfWork.AllocationRepository.GetAllocationsByEmployeeIdAsync(employeeId);
        return _mapper.Map<IEnumerable<AllocationDto>>(allocations);
    }


    public async Task<IEnumerable<AllocationDto>> GetAllocationsByProjectIdAsync(int projectId)
    {
        var allocations = await _unitOfWork.AllocationRepository.GetAllocationsByProjectIdAsync(projectId);
        return _mapper.Map<IEnumerable<AllocationDto>>(allocations);
    }

    public async Task<bool> SoftDeleteAllocationAsync(int id)
    {
        var allocation = await _unitOfWork.AllocationRepository.GetByIdAsync(id);
        if (allocation == null || allocation.IsDeleted)
        {
            return false;
        }

        var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(allocation.EmployeeId);
        employee.TotalAllocatedPercentage -= allocation.AllocationPercentage;

        var hasActiveAllocations = await _unitOfWork.AllocationRepository
            .GetAllQueryable()
            .AnyAsync(a => a.EmployeeId == employee.EmployeeId && a.AllocationId != id && !a.IsDeleted);

        employee.IsBench = !hasActiveAllocations;
        if (employee.TotalAllocatedPercentage == 0 && employee.IsBench == true)
        {
            await _notificationService.CreateNotificationAsync("Bench Status", $"Employee {employee.FirstName} {employee.MidName} {employee.LastName} is on the bench.");
        }
        allocation.IsDeleted = true;
        _unitOfWork.AllocationRepository.Update(allocation);
        _unitOfWork.EmployeeRepository.Update(employee);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<(List<AllocationDto> Allocations, int TotalCount)> GetAllocationTableAsync(int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
    {
        var query = _unitOfWork.AllocationRepository.GetAllQueryable();
        searchValue = searchValue.ToLower().Trim();
        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(a => a.Employee.FirstName.ToLower().Contains(searchValue) ||
                                     a.Employee.LastName.ToLower().Contains(searchValue) ||
                                     a.Project.Name.Contains(searchValue) ||
                                     a.Level.NameEn.ToLower().Contains(searchValue) ||
                                     a.Position.NameEn.ToLower().Contains(searchValue));
        }

        var totalCount = await query.CountAsync();

        if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
        {
            query = sortColumn switch
            {
                "EmployeeName" => sortColumnDirection == "asc" ? query.OrderBy(a => a.Employee.FirstName) : query.OrderByDescending(a => a.Employee.FirstName),
                "LevelName" => sortColumnDirection == "asc" ? query.OrderBy(a => a.Level.NameEn) : query.OrderByDescending(a => a.Level.NameEn),
                "PositionName" => sortColumnDirection == "asc" ? query.OrderBy(a => a.Position.NameEn) : query.OrderByDescending(a => a.Position.NameEn),
                "ProjectName" => sortColumnDirection == "asc" ? query.OrderBy(a => a.Project.Name) : query.OrderByDescending(a => a.Project.Name),
                "AllocationPercentage" => sortColumnDirection == "asc" ? query.OrderBy(a => a.AllocationPercentage) : query.OrderByDescending(a => a.AllocationPercentage),
                "StartDate" => sortColumnDirection == "asc" ? query.OrderBy(a => a.StartDate) : query.OrderByDescending(a => a.StartDate),
                "EndDate" => sortColumnDirection == "asc" ? query.OrderBy(a => a.EndDate) : query.OrderByDescending(a => a.EndDate),
                _ => query.OrderBy(a => a.Employee.FirstName),
            };
        }

        var allocations = await query.Skip(start).Take(length).ToListAsync();

        return (_mapper.Map<List<AllocationDto>>(allocations), totalCount);
    }

    public async Task<IEnumerable<EmployeeAllocationDto>> GetEmployeesForAllocationAsync(int levelId, int positionId, int allocationPercentage, int projectId)
    {
        var employees = await _unitOfWork.AllocationRepository.GetEmployeesWithAvailableAllocationAsync(levelId, positionId, allocationPercentage, projectId);
        return _mapper.Map<IEnumerable<EmployeeAllocationDto>>(employees);
    }



    public async Task<AllocationDto> EditAllocationAsync(EditAllocationDto allocationDto)
    {

        var validatore = new EditAllocationDtoValidator(_unitOfWork);

        var validationResult = await validatore.ValidateAsync(allocationDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var allocation = await _unitOfWork.AllocationRepository.GetByIdAsync(allocationDto.AllocationId);
        if (allocation == null)
        {
            throw new KeyNotFoundException($"Allocation with ID {allocationDto.AllocationId} not found.");
        }

        _mapper.Map(allocationDto, allocation);

        var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(allocation.EmployeeId);

        var existingAllocations = await _unitOfWork.AllocationRepository.GetAllocationsByEmployeeIdAsync(allocation.EmployeeId);
        int totalAllocation = existingAllocations
        .Where(a => a.AllocationId != allocationDto.AllocationId)
        .Sum(a => a.AllocationPercentage);
        employee.TotalAllocatedPercentage = totalAllocation + allocationDto.AllocationPercentage;
        employee.IsBench = employee.TotalAllocatedPercentage == 0;

        _unitOfWork.EmployeeRepository.Update(employee);
        _unitOfWork.AllocationRepository.Update(allocation);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<AllocationDto>(allocation);
    }


    public async Task<IEnumerable<Project>> GetFilteredProjectsAsync(int employeeId, int currentAllocationId)
    {
        var allProjects = await _unitOfWork.ProjectRepository.GetAllAsync();
        var allocatedProjects = await _unitOfWork.AllocationRepository.GetAllocationsByEmployeeIdAsync(employeeId);

        var allocatedProjectIds = allocatedProjects
            .Where(a => a.AllocationId != currentAllocationId)
            .Select(a => a.projectId)
            .ToList();

        return allProjects.Where(p => !allocatedProjectIds.Contains(p.ProjectId)).ToList();
    }

    /// <summary>
    public async Task<int> TotalOfSharedResource()
    {
        var SharedResource = await _unitOfWork.AllocationRepository.SharedResource();
              return SharedResource;
    }
    public async Task<Dictionary<string, Dictionary<string, int>>> GetEmployeeDistributionByProjectAsync()
    {
        var allocations = await _unitOfWork.AllocationRepository.GetAllIncludingAsync(a => a.Project, a => a.Level);
        allocations = allocations.Where(x => !x.IsDeleted);

        var distribution = allocations
            .GroupBy(a => a.Project.Name)
            .ToDictionary(
                g => g.Key,
                g => g.GroupBy(a => a.Level.NameEn)
                      .ToDictionary(gg => gg.Key, gg => gg.Count())
            );
        return distribution;
    }
    public async Task<List<AllocationReportDto>> GetAllocationsFilteredReport(DateTime startDate, DateTime endDate, string search, int page, int pageSize)
    {
        var query = _unitOfWork.AllocationRepository.GetAllQueryable()
            .Where(e => e.StartDate >= startDate && e.EndDate <= endDate);
        query = query.Include(x => x.Employee).Include(x => x.Project).Include(x => x.Position);
        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower().Trim();
            query = query.Where(e =>
                 (e.Employee.FirstName.ToLower() + " " +
                 (e.Employee.MidName.ToLower() + " ") +
                 (e.Employee.FourthName.ToLower() + " ") +
                 e.Employee.LastName.ToLower()).Contains(search) ||
                 e.Project.Name.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync();

        var projects = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var AllocationReportDtos = _mapper.Map<List<AllocationReportDto>>(projects);

        foreach (var dto in AllocationReportDtos)
        {
            dto.FormattedStartDate = dto.StartDate.ToString("dd/MM/yyyy");
            dto.FormattedEndDate = dto.EndDate.HasValue ? dto.EndDate.Value.ToString("dd/MM/yyyy") : "N/A";
        }
        return AllocationReportDtos;
    }

    public async Task<IEnumerable<AllocationHistoryDto>> GetEmployeeAllocationHistoryAsync(int employeeId, string searchValue, string sortColumn, string sortColumnDirection)
    {
        var allocations = await _unitOfWork.AllocationRepository.GetEmployeeAllocationHistory(employeeId);

        searchValue = searchValue.ToLower();

        if (!string.IsNullOrEmpty(searchValue))
        {
            allocations = allocations.Where(e => e.Employee.FirstName.ToLower().Contains(searchValue)
            || e.Employee.LastName.ToLower().Contains(searchValue)
            || e.Level.NameEn.ToLower().Contains(searchValue)
            || e.Position.NameEn.ToLower().Contains(searchValue)
            || e.AllocationPercentage.ToString().ToLower().Contains(searchValue)
            || e.StartDate.ToString().ToLower().Contains(searchValue)
            || e.EndDate.ToString().ToLower().Contains(searchValue)
            || e.Project.Name.ToLower().Contains(searchValue));

        }

        if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
        {
            allocations = sortColumn switch
            {
                "EmployeeName" => sortColumnDirection == "asc" ? allocations.OrderBy(a => a.Employee.FirstName) : allocations.OrderByDescending(a => a.Employee.FirstName),
                "LevelName" => sortColumnDirection == "asc" ? allocations.OrderBy(a => a.Level.NameEn) : allocations.OrderByDescending(a => a.Level.NameEn),
                "PositionName" => sortColumnDirection == "asc" ? allocations.OrderBy(a => a.Position.NameEn) : allocations.OrderByDescending(a => a.Position.NameEn),
                "ProjectName" => sortColumnDirection == "asc" ? allocations.OrderBy(a => a.Project.Name) : allocations.OrderByDescending(a => a.Project.Name),
                "AllocationPercentage" => sortColumnDirection == "asc" ? allocations.OrderBy(a => a.AllocationPercentage) : allocations.OrderByDescending(a => a.AllocationPercentage),
                "StartDate" => sortColumnDirection == "asc" ? allocations.OrderBy(a => a.StartDate) : allocations.OrderByDescending(a => a.StartDate),
                "EndDate" => sortColumnDirection == "asc" ? allocations.OrderBy(a => a.EndDate) : allocations.OrderByDescending(a => a.EndDate),
                _ => allocations.OrderBy(a => a.Employee.FirstName),
            };
        }

        var allocationHistoryDtos = _mapper.Map<List<AllocationHistoryDto>>(allocations);

        return allocationHistoryDtos;
    }


}

