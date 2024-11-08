using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using static DataLayer.Enums.LookUpEnums;
namespace BusinessLayer.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<(IEnumerable<DisplayProjectDto> Projects, int TotalCount)> GetProjectsTableAsync(int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
        {
            var query = _unitOfWork.ProjectRepository.GetAllQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                query = query.Where(p => p.Name.Contains(searchValue) ||
                                         p.Status.NameEn.Contains(searchValue) 
                                  
                                         );
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                switch (sortColumn.ToLower())
                {
                    case "name":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                        break;

                    case "statusname":  // Sort by StatusName
                        query = sortColumnDirection.ToLower() == "asc"
                            ? query.OrderBy(p => p.Status.NameEn)
                            : query.OrderByDescending(p => p.Status.NameEn);
                        break;

                    case "startdate":
                        query = sortColumnDirection.ToLower() == "asc"
                            ? query.OrderBy(p => p.StartDate)
                            : query.OrderByDescending(p => p.StartDate);
                        break;

                    case "enddate":
                        query = sortColumnDirection.ToLower() == "asc"
                            ? query.OrderBy(p => p.EndDate)
                            : query.OrderByDescending(p => p.EndDate);
                        break;
                    default:
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                        break;
                }
            }
            var totalRecords = await query.CountAsync();

            var paginatedProjects = await query.Skip(start).Take(length).ToListAsync();

            var projectDto = _mapper.Map<IEnumerable<DisplayProjectDto>>(paginatedProjects);

            return (projectDto, totalRecords);
        }



        public async Task<(IEnumerable<DisplayProjectDto> Projects, int TotalCount)> GetDeletedProjectsTableAsync(int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
        {
            var query = _unitOfWork.ProjectRepository.GetDeletedQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                query = query.Where(p => p.Name.Contains(searchValue) ||
                                         p.Status.NameEn.Contains(searchValue) 
                                        

                                         );
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                switch (sortColumn.ToLower())
                {
                    case "name":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                        break;

                    case "statusname":  // Sort by StatusName
                        query = sortColumnDirection.ToLower() == "asc"
                            ? query.OrderBy(p => p.Status.NameEn)
                            : query.OrderByDescending(p => p.Status.NameEn);
                        break;

                    case "startdate":
                        query = sortColumnDirection.ToLower() == "asc"
                            ? query.OrderBy(p => p.StartDate)
                            : query.OrderByDescending(p => p.StartDate);
                        break;

                    case "enddate":
                        query = sortColumnDirection.ToLower() == "asc"
                            ? query.OrderBy(p => p.EndDate)
                            : query.OrderByDescending(p => p.EndDate);
                        break;
                    default:
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                        break;
                }
            }
            var totalRecords = await query.CountAsync();

            var paginatedProjects = await query.Skip(start).Take(length).ToListAsync();

            var projectDto = _mapper.Map<IEnumerable<DisplayProjectDto>>(paginatedProjects);

            return (projectDto, totalRecords);
        }


        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.GenericRepository<Project>()
                .GetAllAsyncByBoolFilter(p => !p.IsDeleted);

            return _mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }

        public async Task<IEnumerable<DisplayProjectDto>> GetProjectsListAsync()
        {
            var projects = await _unitOfWork.ProjectRepository.GetAllProjects();

            return _mapper.Map<IEnumerable<DisplayProjectDto>>(projects);

        }

        public async Task<IEnumerable<DisplayProjectDto>> GetProjectsDeletedListAsync()
        {
            var projects = await _unitOfWork.ProjectRepository.GetDeletedProjectsAsync();

            return _mapper.Map<IEnumerable<DisplayProjectDto>>(projects);

        }

        public async Task<ProjectDTO> GetProjectByIdAsync(int projectId)
        {
            var project = await _unitOfWork.ProjectRepository.GetProjectWithAllocationsAsync(projectId);
            return _mapper.Map<ProjectDTO>(project);
        }



        public async Task AddProjectAsync(ProjectDTO projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            await _unitOfWork.GenericRepository<Project>().AddAsync(project);
            await _unitOfWork.CompleteAsync();
        }


        public async Task UpdateProjectAsync(ProjectDTO projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            _unitOfWork.GenericRepository<Project>().Update(project);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<bool> SoftDeleteProjectAsync(int projectId)
        {
            var project = await _unitOfWork.ProjectRepository.GetProjectWithAllocationsAsync(projectId);

            if (project == null || project.IsDeleted)
            {
                return false;
            }

            await _unitOfWork.ProjectRepository.SoftDeleteProjectAsync(projectId);

            foreach (var allocation in project.allocations)
            {
                var employee = allocation.Employee;

                var hasActiveAllocations = await _unitOfWork.AllocationRepository
                    .GetAllQueryable()
                    .AnyAsync(a => a.EmployeeId == employee.EmployeeId && !a.IsDeleted);

                employee.IsBench = !hasActiveAllocations;
                
                if (employee.TotalAllocatedPercentage == 0 && employee.IsBench)
                {
                    await _notificationService.CreateNotificationAsync("Bench Status",
                        $"Employee {employee.FirstName} {employee.MidName} {employee.LastName} is on the bench.");
                }
            }
            await _unitOfWork.ProjectRepository.SoftDeleteProjectAsync(projectId);


            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> RestoreProjectAsync(int projectId)
        {
            await _unitOfWork.ProjectRepository.RestoreProjectAsync(projectId);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<int> GetTotalProjectsCountAsync()
        {
            var projects = await _unitOfWork.GenericRepository<Project>()
                .GetAllAsyncByBoolFilter(p => !p.IsDeleted);
            return projects.Count();
        }
        public async Task<IEnumerable<ProjectStatusCountDto>> GetProjectStatusCountsAsync()
        {
            var projects = await _unitOfWork.ProjectRepository.GetAllAsync();
            projects = projects.Where(x => !x.IsDeleted);
            return projects.GroupBy(x => x.StatusId)
                .Select(x => new ProjectStatusCountDto
                {
                    Status = Enum.GetName(typeof(StatusCategory), x.Key),
                    Count = x.Count()
                });

        }


        public async Task<IEnumerable<ProjectDTO>> GetProjectsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var project = await _unitOfWork.ProjectRepository.GetProjectsByDateRangeAsync(startDate, endDate);
            var projectDto = _mapper.Map<IEnumerable<ProjectDTO>>(project);
            return projectDto;
        }
        public async Task<List<ProjectReportDto>> GetProjectsFilteredReport(DateTime startDate, DateTime endDate, string search, int page, int pageSize)
        {

            startDate = startDate.Date;
            endDate = endDate.Date;

            var query = _unitOfWork.ProjectRepository.GetAllQueryable()
                .Where(e => e.StartDate >= startDate && e.EndDate <= endDate);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                query = query.Where(e =>
                    e.Name.ToLower().Contains(search) ||
                    e.Status.NameEn.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            var projects = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to DTO
            var projectDtos = _mapper.Map<List<ProjectReportDto>>(projects);

            foreach (var dto in projectDtos)
            {
                dto.FormattedStartDate = dto.StartDate.ToString("dd/MM/yyyy");
                dto.FormattedEndDate = dto.EndDate.HasValue ? dto.EndDate.Value.ToString("dd/MM/yyyy") : "N/A";
            }
            return projectDtos;
        }

        public async Task<DisplayProjectDto> GetProjectWithAllocationsAsync(int projectId)
        {

            var projects = await _unitOfWork.ProjectRepository.GetProjectWithAllocationsAsync(projectId);

            return _mapper.Map<DisplayProjectDto>(projects);
        }
    }
}
