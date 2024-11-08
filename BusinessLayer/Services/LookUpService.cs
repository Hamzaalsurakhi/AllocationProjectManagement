using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interface;

namespace BusinessLayer.Repositories
{
    public class LookUpService : ILookUpService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LookUpService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LookUpDto>> GetLookUpsByCategoryAsync(LookUpEnums.CategoryCode category)
        {
            var lookUps = await _unitOfWork.LookUpRepository.GetLookUpsByCategoryAsync(category);
            return _mapper.Map<IEnumerable<LookUpDto>>(lookUps);
        }
        public async Task<IEnumerable<LookUpDto>> GetAllLevelsAsync()
        {
            var lookUps = await _unitOfWork.LookUpRepository.GetAllLevels();
            return _mapper.Map<IEnumerable<LookUpDto>>(lookUps);
        }
        public async Task<IEnumerable<LookUpDto>> GetAllPositionsAsync()
        {
            var lookUps = await _unitOfWork.LookUpRepository.GetAllPositions();
            return _mapper.Map<IEnumerable<LookUpDto>>(lookUps);
        }
        public async Task<IEnumerable<LookUpDto>> GetAllStatusesAsync()
        {
            var lookUps = await _unitOfWork.LookUpRepository.GetAllStatuses();
            return _mapper.Map<IEnumerable<LookUpDto>>(lookUps);
        }
        public async Task<IEnumerable<LookUpDto>> GetAllTeamCountriesAsync()
        {
            var lookUps = await _unitOfWork.LookUpRepository.GetAllTeamCountries();
            return _mapper.Map<IEnumerable<LookUpDto>>(lookUps);
        }

    }
}