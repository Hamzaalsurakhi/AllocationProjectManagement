using DataLayer.Enums;
using FluentValidation;

namespace BusinessLayer.DTOs
{
    public class AddAllocationDto
    {
        public int ProjectId { get; set; }
        public int[] SelectedEmployeeIds { get; set; }
        public int AllocationPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? LevelId { get; set; }
        public int? PositionId { get; set; }

    }
    public class AddAllocationDtoValidator : AbstractValidator<AddAllocationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddAllocationDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleForEach(x => x.SelectedEmployeeIds)
            .MustAsync(BeValidAllocation)
            .WithMessage("Employee {PropertyValue} cannot be allocated more than 100%.");

            RuleFor(x => x)
                .MustAsync(HasValidProjectManager)
                .When(x => x.LevelId == (int)LookUpEnums.levelCategory.ProjectManager)
                .WithMessage("This project already has a Project Manager or you are trying to Allocate more that one.");

            RuleFor(x => x)
                .MustAsync(HasValidTeamLeader)
                .When(x => x.LevelId == (int)LookUpEnums.levelCategory.TeamLeader)
                .WithMessage("This project already has a Team Leader or you are trying to allocate more than one.");
        }

        private async Task<bool> BeValidAllocation(int employeeId, CancellationToken token)
        {
            var existingAllocations = await _unitOfWork.AllocationRepository.GetAllocationsByEmployeeIdAsync(employeeId);
            int totalAllocation = existingAllocations.Sum(a => a.AllocationPercentage);
            return totalAllocation < 100;
        }

        private async Task<bool> HasValidProjectManager(AddAllocationDto allocationDto, CancellationToken token)
        {
            var isProjectManagerAllocated = await _unitOfWork.AllocationRepository.IsProjectManagerAllocatedAsync(allocationDto.ProjectId);
            if (isProjectManagerAllocated)
            {
                return false;
            }
            var selectedProjectManagers = 0;
            foreach (var employeeId in allocationDto.SelectedEmployeeIds)
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
                if (employee.LevelId == (int)LookUpEnums.levelCategory.ProjectManager)
                {
                    selectedProjectManagers++;
                    if (selectedProjectManagers > 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private async Task<bool> HasValidTeamLeader(AddAllocationDto allocationDto, CancellationToken token)
        {
            var isTeamLeaderAllocated = await _unitOfWork.AllocationRepository.IsTeamLeaderAllocatedAsync(allocationDto.ProjectId);
            if (isTeamLeaderAllocated)
            {
                return false;
            }

            var selectedTeamLeadersCount = 0;
            foreach (var employeeId in allocationDto.SelectedEmployeeIds)
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
                if (employee.LevelId == (int)LookUpEnums.levelCategory.TeamLeader)
                {
                    selectedTeamLeadersCount++;
                    if (selectedTeamLeadersCount > 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
