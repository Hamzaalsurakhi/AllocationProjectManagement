using DataLayer.Entities;

namespace DataLayer.Interface
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetAllProjects();
        IQueryable<Project> GetAllQueryable();
        IQueryable<Project> GetDeletedQueryable();

        Task<IEnumerable<Project>> GetDeletedProjectsAsync();
        Task<Project> GetProjectWithAllocationsAsync(int projectId);
        Task<IEnumerable<Project>> GetProjectsByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<bool> SoftDeleteProjectAsync(int projectId);

        Task<bool> RestoreProjectAsync(int projectId);

    }

}
