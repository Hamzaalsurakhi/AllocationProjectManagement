using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ViewModels
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }

        [UniqueProjectName]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public int StatusId { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifyOn { get; set; }
        public int? ModifyBy { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;


        public IEnumerable<SelectListItem>? Statuses { get; set; }


    }

    public class ProjectValidator : AbstractValidator<ProjectViewModel>
    {
        public ProjectValidator()
        {
            RuleFor(p => p.Name)
                //.NotEmpty().WithMessage("Name is required.")
                .MaximumLength(250).WithMessage("Name cannot exceed 250 characters.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters.");

            RuleFor(p => p.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(p => p)
           .Custom(( p, context ) =>
           {
               if (p.EndDate.HasValue && p.StartDate.HasValue && p.EndDate < p.StartDate)
                   {
                   context.AddFailure("EndDate", "End date cannot be earlier than the start date.");
                   }
           });
            RuleFor(p => p)
       .Custom(( p, context ) =>
       {
           if (p.EndDate.HasValue && p.StartDate.HasValue && p.EndDate < p.StartDate)
               {
               context.AddFailure("StartDate", "Start date cannot be Greater than the end date.");
               }
       });


            RuleFor(p => p.StatusId)
                .GreaterThan(0).WithMessage("Please select a valid status.");


        }
    }
}
