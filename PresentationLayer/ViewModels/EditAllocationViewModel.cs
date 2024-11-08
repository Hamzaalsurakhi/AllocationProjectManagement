using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.ViewModels
{
    public class EditAllocationViewModel
    {
        public int AllocationId { get; set; }
        public int projectId { get; set; }

        public int EmployeeId { get; set; }

        public int AllocationPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int LevelId { get; set; }

        public int PositionId { get; set; }


        public IEnumerable<SelectListItem>? Projects { get; set; }
    }


    public class EditAllocationValidator : AbstractValidator<EditAllocationViewModel>
    {
        public EditAllocationValidator()
        {
            RuleFor(x => x.projectId)
            .NotNull()
            .WithMessage("Project is required.");

            RuleFor(x => x.AllocationPercentage)
                .NotNull()
                .InclusiveBetween(1, 100)
                .WithMessage("Allocation percentage must be between 1 and 100.");

            RuleFor(x => x.StartDate)
                .NotNull()
                .WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
                .NotNull().WithMessage("End Date is Required")
                .Must((model, endDate) => !endDate.HasValue || endDate >= model.StartDate)
                .WithMessage("End date cannot be before the start date.");


        }

    }
}
