using DataLayer.Data;
using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class LookUpRepository : GenericRepository<LookUp>, ILookUpRepository
    {

        private readonly AppDbContext _appDbContext;
        public LookUpRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<LookUp>> GetLookUpsByCategoryAsync(LookUpEnums.CategoryCode category)
        {
            return await _appDbContext.LookUps
                .Where(x => x.LookupCategoryId == (int)category && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<LookUp>> GetAllPositions()
        {
            return await _appDbContext.LookUps
                .Where(x => x.LookupCategoryId == (int)LookUpEnums.CategoryCode.Position && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<LookUp>> GetAllLevels()
        {
            return await _appDbContext.LookUps
                .Where(x => x.LookupCategoryId == (int)LookUpEnums.CategoryCode.Level && !x.IsDeleted)
                .ToListAsync();
        }
        public async Task<IEnumerable<LookUp>> GetAllStatuses()
        {
            return await _appDbContext.LookUps
                .Where(x => x.LookupCategoryId == (int)LookUpEnums.CategoryCode.Status && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<LookUp>> GetAllTeamCountries()
        {
            return await _appDbContext.LookUps
                .Where(x => x.LookupCategoryId == (int)LookUpEnums.CategoryCode.TeamCountry && !x.IsDeleted)
                .ToListAsync();
        }
    }
}
