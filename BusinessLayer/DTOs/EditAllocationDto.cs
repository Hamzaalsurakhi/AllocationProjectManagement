using DataLayer.Enums;
using FluentValidation;

namespace BusinessLayer.DTOs
{
    public class EditAllocationDto
    {
        public int AllocationId { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public int AllocationPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int LevelId { get; set; }
        public int PositionId { get; set; }

    }

    public class EditAllocationDtoValidator : AbstractValidator<EditAllocationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public EditAllocationDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.AllocationId)
                .MustAsync(ExistAsync)
                .WithMessage("Allocation with ID {PropertyValue} not found.");

            RuleFor(x => x.AllocationPercentage)
                .MustAsync(BeValidAllocationPercentageAsync)
                .WithMessage("Employee cannot be allocated more than 100%.");

            RuleFor(x => x.LevelId)
                .MustAsync(BeValidProjectManagerAllocationAsync)
                .When(x => x.LevelId == (int)LookUpEnums.levelCategory.ProjectManager)
                .WithMessage("This project already has a Project Manager.");

            RuleFor(x => x.LevelId)
                .MustAsync(BeValidTeamLeaderAllocationAsync)
                .When(x => x.LevelId == (int)LookUpEnums.levelCategory.TeamLeader)
                .WithMessage("This project already has a Team Leader.");

        }

        private async Task<bool> ExistAsync(int allocationId, CancellationToken token)
        {
            var allocation = await _unitOfWork.AllocationRepository.GetByIdAsync(allocationId);
            return allocation != null;
        }

        private async Task<bool> BeValidAllocationPercentageAsync(EditAllocationDto allocationDto, int newAllocationPercentage, CancellationToken token)
        {
            var allocation = await _unitOfWork.AllocationRepository.GetByIdAsync(allocationDto.AllocationId);
            var existingAllocations = await _unitOfWork.AllocationRepository.GetAllocationsByEmployeeIdAsync(allocation.EmployeeId);

            int totalAllocation = existingAllocations
                .Where(a => a.AllocationId != allocationDto.AllocationId)
                .Sum(a => a.AllocationPercentage);

            return totalAllocation + newAllocationPercentage <= 100;
        }

        private async Task<bool> BeValidProjectManagerAllocationAsync(EditAllocationDto allocationDto, int levelId, CancellationToken token)
        {
            var allocation = await _unitOfWork.AllocationRepository.GetByIdAsync(allocationDto.AllocationId);
            bool isProjectManagerAllocated = await _unitOfWork.AllocationRepository.IsProjectManagerAllocatedAsync(allocationDto.ProjectId);

            return !isProjectManagerAllocated || allocation.projectId == allocationDto.ProjectId;
        }

        private async Task<bool> BeValidTeamLeaderAllocationAsync(EditAllocationDto allocationDto, int levelId, CancellationToken token)
        {
            var allocation = await _unitOfWork.AllocationRepository.GetByIdAsync(allocationDto.AllocationId);
            bool isTeamLeaderAllocated = await _unitOfWork.AllocationRepository.IsTeamLeaderAllocatedAsync(allocationDto.ProjectId);

            return !isTeamLeaderAllocated || allocation.projectId == allocationDto.ProjectId;
        }
    }
}
