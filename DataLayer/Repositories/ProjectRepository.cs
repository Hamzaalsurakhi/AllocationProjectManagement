using DataLayer.Data;
using DataLayer.Entities;
using DataLayer.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProjectRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await _appDbContext.Projects.Include(p => p.Status).Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetDeletedProjectsAsync()
        {
            return await _appDbContext.Projects.Include(p => p.Status).Where(p => p.IsDeleted).ToListAsync();
        }

        public IQueryable<Project> GetAllQueryable()
        {
            return _appDbContext.Projects.Where(P => !P.IsDeleted).Include(P => P.Status).AsQueryable();
        }

        public IQueryable<Project> GetDeletedQueryable()
        {
            return _appDbContext.Projects.Where(P => P.IsDeleted).Include(P => P.Status).AsQueryable();
        }



        public async Task<IEnumerable<Project>> GetProjectsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _appDbContext.projects.Where(p => p.StartDate >= startDate && p.EndDate <= endDate).ToListAsync();
        }


        public async Task<Project> GetProjectWithAllocationsAsync(int projectId)
        {
            return await _appDbContext.Projects
                .Include(p => p.allocations.Where(a => !a.IsDeleted)) // Filter allocations by IsDeleted = false
                .ThenInclude(a => a.Employee)
                .ThenInclude(e => e.Level)
                .Include(p => p.allocations.Where(a => !a.IsDeleted)) // Ensure the same filter applies for other related includes
                .ThenInclude(a => a.Employee)
                .ThenInclude(e => e.Position)
                .Where(p => p.ProjectId == projectId && !p.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SoftDeleteProjectAsync(int projectId)
        {
            var project = await _appDbContext.Projects
                .Include(p => p.allocations)
                .ThenInclude(a => a.Employee) 
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project == null || project.IsDeleted)
            {
                return false;
            }

            project.IsDeleted = true;

            foreach (var allocation in project.allocations)
            {
                if (!allocation.IsDeleted)
                {
                    allocation.IsDeleted = true;

                    allocation.Employee.TotalAllocatedPercentage -= allocation.AllocationPercentage;

                    if (allocation.Employee.TotalAllocatedPercentage == 0)
                    {
                        allocation.Employee.TotalAllocatedPercentage = 0; 
                        allocation.Employee.IsBench = true;
                    }
                }
            }

            await _appDbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> RestoreProjectAsync(int projectId)
        {
            var project = await _appDbContext.Projects
                .FindAsync(projectId);

            if (project == null)
            {
                return false;
            }

            // Restore the project
            project.IsDeleted = false;

            _appDbContext.Projects.Update(project);

            await _appDbContext.SaveChangesAsync();

            return true;
        }


    }

}


