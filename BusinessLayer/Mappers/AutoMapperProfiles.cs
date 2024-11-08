using AutoMapper;
using BusinessLayer.DTOs;
using DataLayer.Entities;

namespace BusinessLayer.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();

            CreateMap<Employee, DisplayEmployeeDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.MidName} {src.FourthName} {src.LastName}".Trim()))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.NameEn))
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.NameEn))
            .ForMember(dest => dest.TeamCountry, opt => opt.MapFrom(src => src.TeamCountry.NameEn))
            .ForMember(dest => dest.ActiveAllocations, opt => opt.MapFrom(src => src.ProjectAllocation.Where(a => a.EndDate >= DateTime.UtcNow)))
            .ReverseMap();

            CreateMap<EmployeeDataTableDto, Employee>().ReverseMap();

            CreateMap<Project, ProjectDTO>()
                .ForMember(dest => dest.Allocations, opt => opt.MapFrom(src => src.allocations)).ReverseMap(); 

            CreateMap<Project, ProjectReportDto>()
           .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.NameEn))
           .ReverseMap();

            CreateMap<Project, DisplayProjectDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.NameEn))
            .ForMember(dest => dest.Allocations, opt => opt.MapFrom(src => src.allocations))
            .ReverseMap();

            CreateMap<LookUp, LookUpDto>().ReverseMap();

            CreateMap<UserDto, User>().ReverseMap();

            CreateMap<Notification, NotificationDto>().ReverseMap();




            CreateMap<Allocation, AllocationDto>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
            .ForMember(dest => dest.LevelId, opt => opt.MapFrom(src => src.LevelId))
            .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId))
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.NameEn))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.NameEn));

            CreateMap<AddAllocationDto, Allocation>();

            CreateMap<Employee, EmployeeAllocationDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));


            CreateMap<Allocation, EditAllocationDto>().ReverseMap();

            CreateMap<Allocation, AllocationReportDto>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.MidName} {src.Employee.FourthName} {src.Employee.LastName}".Trim()))
                .ReverseMap();

            CreateMap<Allocation, AllocationDetailsDto>()
                 .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName))
                 .ForMember(dest => dest.LevelNameEn, opt => opt.MapFrom(src => src.Employee.Level.NameEn))
                 .ForMember(dest => dest.PositionNameEn, opt => opt.MapFrom(src => src.Employee.Position.NameEn));

            CreateMap<Allocation, AllocationHistoryDto>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.NameEn))
                .ForMember(dest => dest.levelName, opt => opt.MapFrom(src => src.Level.NameEn))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate ?? DateTime.Now));

            
        }
    }
}





