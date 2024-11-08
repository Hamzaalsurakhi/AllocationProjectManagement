using BusinessLayer.DTOs;
using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ILookUpService 
    {
        Task<IEnumerable<LookUpDto>> GetLookUpsByCategoryAsync(LookUpEnums.CategoryCode category);
        Task<IEnumerable<LookUpDto>> GetAllLevelsAsync();
        Task<IEnumerable<LookUpDto>> GetAllPositionsAsync();
        Task<IEnumerable<LookUpDto>> GetAllStatusesAsync();
        Task<IEnumerable<LookUpDto>> GetAllTeamCountriesAsync();
    }
}
