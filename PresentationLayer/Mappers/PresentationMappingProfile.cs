using AutoMapper;
using BusinessLayer.DTOs;
using PresentationLayer.ViewModels;

namespace PresentationLayer.Mappers;

public class MapperToBusiness : Profile
{
    public MapperToBusiness()
    {
        CreateMap<EmployeeDto, EmployeeViewModel>().ReverseMap();

        CreateMap<DisplayEmployeeDto, EmployeeViewModel>().ReverseMap();

        CreateMap<DisplayEmployeeDto, DisplayEmployeeViewModel>().ReverseMap();

        CreateMap<ProjectDTO, ProjectViewModel>().ReverseMap();

        CreateMap<DisplayProjectDto, DisplayProjectViewModel>().ReverseMap();

        CreateMap<UserViewModel, UserDto>().ReverseMap();

        CreateMap<ResetPasswordDto, ResetPasswordViewModel>().ReverseMap();

        CreateMap<EmployeeWithTimeDifferenceDto, EmployeeWithTimeDifferenceViewModel>().ReverseMap();

        CreateMap<NotificationViewModel, NotificationDto>().ReverseMap();


        CreateMap<CreateAllocationViewModel, AddAllocationDto>().ReverseMap();

        CreateMap<AllocationViewModel, AllocationDto>().ReverseMap();

        CreateMap<AllocationDto, EditAllocationViewModel>().ReverseMap();

        CreateMap<EditAllocationDto, EditAllocationViewModel>().ReverseMap();

        CreateMap<ProjectDetailsModelView, AllocationDetailsDto>().ReverseMap();

        CreateMap<DisplayProjectDto, DisplayProjectViewModel>()
        .ForMember(dest => dest.Allocations, opt => opt.MapFrom(src => src.Allocations));

        CreateMap<AllocationDetailsDto, AllocationDetailsViewModel>().ReverseMap();


        CreateMap<AddAllocationDto, DisplayEmployeeViewModel>().ReverseMap();


        CreateMap<DisplayEmployeeDto, CreateAllocationViewModel>()
        .ForMember(dest => dest.SelectedEmployeeIds, opt => opt.MapFrom(src => new[] { src.EmployeeId }))
        .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.FullName))
        .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.LevelName))
        .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.PositionName))
        .ForMember(dest => dest.LevelId, opt => opt.MapFrom(src => src.LevelId))
        .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId))
        .ReverseMap();

    }
}
