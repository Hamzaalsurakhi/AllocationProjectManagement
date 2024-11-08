using BusinessLayer.DTOs;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectReportDto>> GetProjectsFilteredReport( DateTime startDate, DateTime endDate, string search, int page, int pageSize );
        Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(); 
        Task<IEnumerable<DisplayProjectDto>> GetProjectsListAsync(); 
        Task<IEnumerable<DisplayProjectDto>> GetProjectsDeletedListAsync();
        Task<ProjectDTO> GetProjectByIdAsync(int projectId);

        Task AddProjectAsync(ProjectDTO ProjectDTO);
        Task UpdateProjectAsync(ProjectDTO ProjectDTO);
       Task<bool> SoftDeleteProjectAsync(int projectId);

        Task<bool> RestoreProjectAsync(int projectId);

        Task<int> GetTotalProjectsCountAsync();
        Task<IEnumerable<ProjectStatusCountDto>> GetProjectStatusCountsAsync();
        Task<IEnumerable<ProjectDTO>> GetProjectsByDateRangeAsync( DateTime startDate, DateTime endDate );
        Task<(IEnumerable<DisplayProjectDto> Projects, int TotalCount)> GetProjectsTableAsync(
                int start,
                int length,
                string searchValue,
                string sortColumn,
                string sortColumnDirection
              
                )
                ;

        Task<(IEnumerable<DisplayProjectDto> Projects, int TotalCount)> GetDeletedProjectsTableAsync(
               int start,
               int length,
               string searchValue,
               string sortColumn,
               string sortColumnDirection
               )
               ;


        Task<DisplayProjectDto> GetProjectWithAllocationsAsync(int projectId);
    }


}
