using DataLayer.Entities;
using DataLayer.Enums;

namespace DataLayer.Interface
{
    public interface ILookUpRepository : IGenericRepository<LookUp>
    {
        Task<IEnumerable<LookUp>> GetAllPositions();
        Task<IEnumerable<LookUp>> GetAllLevels();
        Task<IEnumerable<LookUp>> GetAllStatuses();
        Task<IEnumerable<LookUp>> GetAllTeamCountries();
        Task<IEnumerable<LookUp>> GetLookUpsByCategoryAsync(LookUpEnums.CategoryCode category);

    }
}
