using DataLayer.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ViewModels
{
    public class CreateAllocationViewModel
    {
        public int ProjectId { get; set; }

        public int[]? SelectedEmployeeIds { get; set; }

        [Range(1, 100)]
        public int AllocationPercentage { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public int? LevelId { get; set; }

        public int? PositionId { get; set; }

        public string? EmployeeName { get; set; }
        public string? LevelName { get; set; }
        public string? PositionName { get; set; }

        // These properties are for dropdown lists
        public IEnumerable<SelectListItem>? Employees { get; set; }
        public IEnumerable<SelectListItem>? Projects { get; set; }
        public IEnumerable<SelectListItem>? Levels { get; set; }
        public IEnumerable<SelectListItem>? Positions { get; set; }
    }

    public class CreateAllocationValidator : AbstractValidator<CreateAllocationViewModel>
    {
        public CreateAllocationValidator()
        {
            RuleFor(x => x.ProjectId)
            .NotNull().NotEmpty()
            .WithMessage("Project is required.");

            RuleFor(x => x.SelectedEmployeeIds)
                .NotEmpty()
                .WithMessage("At least one employee must be selected.");

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

            RuleFor(n => n.LevelId)
                .NotNull().WithMessage("Please select a valid level of experience")

                .GreaterThan(0).WithMessage("Please select a valid level of experience");

            RuleFor(n => n.PositionId)
                .NotNull().WithMessage("Please select a valid level of experience")

                .GreaterThan(0).WithMessage("Please select a valid level of experience");
        }

        
    }
}
