using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace BusinessLayer.Services
{
    class EmployeeService : IEmployeeService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService( IMapper mapper, IUnitOfWork unitOfWork )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DisplayEmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAllEmployees();
            return _mapper.Map<IEnumerable<DisplayEmployeeDto>>(employees);
        }
        public async Task<(IEnumerable<DisplayEmployeeDto> Employees, int TotalCount)> GetEmployeesTableAsync(int start, int length, string searchValue, string sortColumn, string sortColumnDirection, string positionId, string levelId)
        {
            var query = _unitOfWork.EmployeeRepository.GetAllQueryable();
            searchValue = searchValue.ToLower().Trim();

            if (!string.IsNullOrEmpty(searchValue))
            { 
                query = query.Where(e =>
                                        (e.FirstName.ToLower() + " " +
                                        (string.IsNullOrEmpty(e.MidName) ? "" : e.MidName.ToLower() + " ") +
                                        (string.IsNullOrEmpty(e.FourthName) ? "" : e.FourthName.ToLower() + " ") +
                                         e.LastName.ToLower()).Contains(searchValue) ||
                                         e.FirstName.Contains(searchValue) ||
                                         e.LastName.Contains(searchValue) ||
                                         e.Email.Contains(searchValue) ||
                                         e.Level.NameEn.Contains(searchValue) ||
                                         e.Position.NameEn.Contains(searchValue) ||
                                         e.PhoneNumber.ToLower().Contains(searchValue)  ||
                                         e.TeamCountry.NameEn.ToLower().Contains(searchValue));
            }

            if (!string.IsNullOrEmpty(positionId))
            {
                if (int.TryParse(positionId, out int posId))
                {
                    query = query.Where(e => e.PositionId == posId);
                }
            }

            
            if (!string.IsNullOrEmpty(levelId))
            {
                if (int.TryParse(levelId, out int lvlId))
                {
                    query = query.Where(e => e.LevelId == lvlId);
                }
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                switch (sortColumn.ToLower())
                {
                    case "fullName":
                        query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(e => e.FirstName + " " + (e.MidName ?? "") + " " + (e.FourthName ?? "") + " " + e.LastName) :
                                query.OrderByDescending(e => e.FirstName + " " + (e.MidName ?? "") + " " + (e.FourthName ?? "") + " " + e.LastName);
                        break;
                    case "firstname":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.FirstName) : query.OrderByDescending(e => e.FirstName);
                        break;
                    case "lastname":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.LastName) : query.OrderByDescending(e => e.LastName);
                        break;
                    case "email":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email);
                        break;
                    case "positionname":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.Position.NameEn) : query.OrderByDescending(e => e.Position.NameEn);
                        break;
                    case "phonenumber":
                        query = sortColumnDirection.ToLower() == "asc"
                            ? query.OrderBy(e => Convert.ToInt64(e.PhoneNumber))
                            : query.OrderByDescending(e => Convert.ToInt64(e.PhoneNumber));
                        break;
                    case "levelname":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.Level.NameEn) : query.OrderByDescending(e => e.Level.NameEn);
                        break;
                    case "teamcountry":
                        query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(e => e.TeamCountry.NameEn) :
                                query.OrderByDescending(e => e.TeamCountry.NameEn);
                        break;

                    case "totalallocatedpercentage":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.TotalAllocatedPercentage) : query.OrderByDescending(e => e.TotalAllocatedPercentage);
                        break;
                    default:
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.FirstName) : query.OrderByDescending(e => e.FirstName);
                        break;

                }
            }
            var totalRecords =  query.Count();

            var paginatedEmployees = await query.Skip(start).Take(length).ToListAsync();

            var employeesDto = _mapper.Map<IEnumerable<DisplayEmployeeDto>>(paginatedEmployees);

            return (employeesDto, totalRecords);
        }



        public async Task<(IEnumerable<DisplayEmployeeDto> Employees, int TotalCount)> GetIsBenchEmployeesTableAsync(int start, int length, string searchValue, string sortColumn, string sortColumnDirection, string positionId, string levelId)
        {
            var query = _unitOfWork.EmployeeRepository.GetAllQueryable();

            query = query.Where(e => e.IsBench && !e.IsDeleted);

            searchValue = searchValue.ToLower().Trim();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(e =>
                                        (e.FirstName.ToLower() + " " +
                                        (string.IsNullOrEmpty(e.MidName) ? "" : e.MidName.ToLower() + " ") +
                                        (string.IsNullOrEmpty(e.FourthName) ? "" : e.FourthName.ToLower() + " ") +
                                         e.LastName.ToLower()).Contains(searchValue) ||
                                         e.FirstName.Contains(searchValue) ||
                                         e.LastName.Contains(searchValue) ||
                                         e.Email.Contains(searchValue) ||
                                         e.Level.NameEn.Contains(searchValue) ||
                                         e.Position.NameEn.Contains(searchValue) ||
                                         e.PhoneNumber.ToLower().Contains(searchValue) ||
                                         e.TeamCountry.NameEn.ToLower().Contains(searchValue));
            }


            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                switch (sortColumn.ToLower())
                {
                    case "fullName":
                        query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(e => e.FirstName + " " + (e.MidName ?? "") + " " + (e.FourthName ?? "") + " " + e.LastName) :
                                query.OrderByDescending(e => e.FirstName + " " + (e.MidName ?? "") + " " + (e.FourthName ?? "") + " " + e.LastName);
                        break;
                    case "email":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email);
                        break;
                    case "positionname":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.Position.NameEn) : query.OrderByDescending(e => e.Position.NameEn);
                        break;
                    case "levelname":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.Level.NameEn) : query.OrderByDescending(e => e.Level.NameEn);
                        break;
                    case "teamcountry":
                        query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(e => e.TeamCountry.NameEn) :
                                query.OrderByDescending(e => e.TeamCountry.NameEn);
                        break;
                    case "phonenumber":
                        query = sortColumnDirection.ToLower() == "asc"
                            ? query.OrderBy(e => Convert.ToInt64(e.PhoneNumber))
                            : query.OrderByDescending(e => Convert.ToInt64(e.PhoneNumber));
                        break;
                    case "totalallocatedpercentage":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.TotalAllocatedPercentage) : query.OrderByDescending(e => e.TotalAllocatedPercentage);
                        break;

                    default:
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.FirstName) : query.OrderByDescending(e => e.FirstName);
                        break;

                }
            }

            var totalRecords = await query.CountAsync();
           
            var paginatedEmployees = await query.Skip(start).Take(length).ToListAsync();

            var employeesDto = _mapper.Map<IEnumerable<DisplayEmployeeDto>>(paginatedEmployees);

            return (employeesDto, totalRecords);
        }



        public async Task<DisplayEmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetEmployeeById(id);
            var activeAllocations = await _unitOfWork.AllocationRepository.GetAllocationsByEmployeeIdAsync(employee.EmployeeId);

            var employeeDto = _mapper.Map<DisplayEmployeeDto>(employee);
            employeeDto.ActiveAllocations = _mapper.Map<IEnumerable<AllocationDto>>(activeAllocations);
            return employeeDto;
        }

        public async Task AddEmployeeAsync( EmployeeDto employeeDto )
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            await _unitOfWork.EmployeeRepository.AddAsync(employee);
            await _unitOfWork.CompleteAsync();
        }

        public async Task AddEmployeesAsync( List<EmployeeDto> employeeDtos )
        {
            var employees = _mapper.Map<List<Employee>>(employeeDtos);
            await _unitOfWork.EmployeeRepository.AddRangeAsync(employees);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateEmployeeAsync(EmployeeDto employeeDto)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeDto.EmployeeId);
            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }
            _mapper.Map(employeeDto, employee);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> SoftDeleteEmployeeAsync(int id)
        {
           await  _unitOfWork.EmployeeRepository.SoftDeleteEmployeeAsync(id);
           await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<int> NumberOfEmployeeInBench()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            employees = employees.Where(x => x.IsBench == true && x.TotalAllocatedPercentage == 0 && !x.IsDeleted);
            return employees.Count();
        }

        public async Task<int> NumberOfSharedResource()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            employees = employees.Where(x => x.IsBench == false && x.TotalAllocatedPercentage != 0);
            return employees.Count();
        }

        public async Task<IQueryable<DisplayEmployeeDto>> GetAllEmployeeIsBench()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            employees = employees.Where(x => x.IsBench);
            var empQuerys = _mapper.Map<IEnumerable<DisplayEmployeeDto>>(employees);
            return empQuerys.Where(x => x.IsBench).AsQueryable();
        }

        public async Task<List<DisplayEmployeeDto>> GetAllEmployeeFilteredReport( DateTime startDate, DateTime endDate, string? search, int page, int pageSize )
        {
            var query = _unitOfWork.EmployeeRepository.GetAllQueryable()
                        .Where(e => e.CreatedOn >= startDate && e.CreatedOn <= endDate);
            search = search?.ToLower().Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(search))
                {
                query = query.Where(e =>
                    e.FirstName.ToLower().Contains(search) ||
                    e.MidName.ToLower().Contains(search) ||
                    e.FourthName.ToLower().Contains(search) ||
                    e.LastName.ToLower().Contains(search) ||
                    e.PhoneNumber.ToLower().Contains(search) ||
                    e.Email.ToLower().Contains(search) ||
                    e.Address.ToLower().Contains(search) ||
                    e.IsBench.ToString().Contains(search) ||
                    e.Position.NameEn.Contains(search) ||
                    e.TotalAllocatedPercentage.ToString().Contains(search) ||
                    e.Level.NameEn.ToLower().Contains(search) ||
                    e.TeamCountry.NameEn.ToLower().Contains(search)||
                    e.Gender.ToLower() == search
                    );
                }

            var totalCount = await query.CountAsync();

            var employees = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var empQuerys = _mapper.Map<List<DisplayEmployeeDto>>(employees);
            foreach (var dto in empQuerys)
            {
                dto.FormattedCreatedOn = dto.CreatedOn.Value.ToString("dd/MM/yyyy");
            }
            return empQuerys;
        }
        public async Task<IEnumerable<EmployeeWithTimeDifferenceDto>> EmployeesBecomeBench()
        {
            var allocations = await _unitOfWork.AllocationRepository.GetAllIncludingAsync(
                a => !a.IsDeleted,
                a => a.Employee, a => a.Project, a => a.Position, a => a.Level);

            var validAllocations = allocations
                    .Where(a => (a.Project.EndDate == null && a.EndDate.Value >= DateTime.Now) ||
                    a.Project.EndDate.Value >= a.EndDate.Value )
                .ToList();

            var employeeAllocations = validAllocations
                .GroupBy(a => a.EmployeeId)
                .Select(g => new
                    {
                    EmployeeId = g.Key,
                    MaxTimeDifference = g.Max(a => (a.EndDate.Value - DateTime.Now).TotalDays),
                    AllocationEndDate = g.Max(a => a.EndDate)  
                    })
                .Where(ea => ea.MaxTimeDifference > 0 && ea.MaxTimeDifference <= 3) 
                .OrderBy(ea => ea.MaxTimeDifference) 
                .Take(6)
                .ToList();

            var employeeIds = employeeAllocations.Select(ea => ea.EmployeeId).ToList();
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            var filteredEmployees = employees
                .Where(e => employeeIds.Contains(e.EmployeeId) && !e.IsDeleted)
                .ToList();

            var topEmployees = employeeAllocations
                .Join(filteredEmployees,
                      ea => ea.EmployeeId,
                      e => e.EmployeeId,
                      ( ea, e ) => new EmployeeWithTimeDifferenceDto
                          {
                          EmployeeId = e.EmployeeId,
                          FirstName = e.FirstName,
                          LastName = e.LastName,
                          PhotoURL = e.PhotoURL,
                          PositionName = e.Position.NameEn,
                          LevelName = e.Level.NameEn,
                          TimeDifference = TimeSpan.FromDays(ea.MaxTimeDifference)
                          })
                .ToList();

            return topEmployees;
        }


        public async Task<(IEnumerable<DisplayEmployeeDto> Employees, int TotalCount)> GetDeletedEmployeesTableAsync(int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
        {
            var query = _unitOfWork.EmployeeRepository.GetDeletedQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                query = query.Where(e =>
                 (e.FirstName.ToLower() + " " +
                 (string.IsNullOrEmpty(e.MidName) ? "" : e.MidName.ToLower() + " ") +
                 (string.IsNullOrEmpty(e.FourthName) ? "" : e.FourthName.ToLower() + " ") +
            e.LastName.ToLower()).Contains(searchValue) ||
                e.FirstName.Contains(searchValue) ||
                                         e.LastName.Contains(searchValue) ||
                                         e.Email.Contains(searchValue) ||
                                         e.Level.NameEn.Contains(searchValue) ||
                                         e.Position.NameEn.Contains(searchValue) ||
                                         e.PhoneNumber.ToLower().Contains(searchValue) ||
                                         e.TeamCountry.NameEn.ToLower().Contains(searchValue)


                                         );
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                switch (sortColumn.ToLower())
                {
                    case "fullName":
                        query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(e => e.FirstName + " " + (e.MidName ?? "") + " " + (e.FourthName ?? "") + " " + e.LastName) :
                                query.OrderByDescending(e => e.FirstName + " " + (e.MidName ?? "") + " " + (e.FourthName ?? "") + " " + e.LastName);
                        break;
                    case "firstname":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.FirstName) : query.OrderByDescending(e => e.FirstName);
                        break;
                    case "lastname":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.LastName) : query.OrderByDescending(e => e.LastName);
                        break;
                    case "email":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email);
                        break;
                    case "positionname":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.Position.NameEn) : query.OrderByDescending(e => e.Position.NameEn);
                        break;
                 
                    case "phonenumber":
                        query = sortColumnDirection.ToLower() == "asc"
                            ? query.OrderBy(e => Convert.ToInt64(e.PhoneNumber))
                            : query.OrderByDescending(e => Convert.ToInt64(e.PhoneNumber));
                        break;

                    case "levelname":
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.Level.NameEn) : query.OrderByDescending(e => e.Level.NameEn);
                        break;
                    case "teamCountry":
                        query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(e => e.TeamCountry.NameEn) :
                                query.OrderByDescending(e => e.TeamCountry.NameEn);
                        break;

                    default:
                        query = sortColumnDirection.ToLower() == "asc" ? query.OrderBy(e => e.FirstName) : query.OrderByDescending(e => e.FirstName);
                        break;

                }
            }
            var totalRecords = await query.CountAsync();

            var paginatedEmployees = await query.Skip(start).Take(length).ToListAsync();

            var employeesDto = _mapper.Map<IEnumerable<DisplayEmployeeDto>>(paginatedEmployees);

            return (employeesDto, totalRecords);
        }

        public async Task<bool> RestoreEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.GenericRepository<Employee>().GetByIdAsync(id);

            if (employee == null)
            {
               
                return false;
            }
            employee.IsDeleted = false;
            _unitOfWork.GenericRepository<Employee>().Update(employee);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<EmployeeDto> GetEmployeeByEmailAsync( string email )
        {
            var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByEmailAsync(email);
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;
        }
    }
}
