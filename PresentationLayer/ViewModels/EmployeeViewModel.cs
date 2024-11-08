using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Validations.UserValidation;
using System.Text.RegularExpressions;

namespace PresentationLayer.ViewModels
{

    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; } 
        public string? MidName { get; set; } = string.Empty;
        public string? FourthName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? PhotoURL { get; set; }
        public IFormFile? Photo { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public bool IsBench { get; set; } = true;
        public int? PositionId { get; set; }
        public int TotalAllocatedPercentage { get; set; }
        public int? LevelId { get; set; }
        public int? TeamCountryId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? Gender { get; set; }

        public IEnumerable<SelectListItem>? Levels { get; set; }
        public IEnumerable<SelectListItem>? Positions { get; set; }
        public IEnumerable<SelectListItem>? TeamCountries { get; set; }

    }
    public class EmployeesValidator : AbstractValidator<EmployeeViewModel>
    {
        private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png" };

        private readonly string _emailPattern = @"^(?=.*[a-z])[a-z\d\._-]+@(gmail|yahoo|techprocess)\.(com|net)$";
        public EmployeesValidator()
        {
            RuleFor(n => n.FirstName)
                .NotEmpty().WithMessage("'First Name' must not be empty")
                .Matches("^[A-Za-z ]+$").WithMessage("'First Name' should contain only letters and spaces.")
                .MaximumLength(50).WithMessage("'First Name' cannot be longer than 50 characters");

            RuleFor(n => n.MidName)
                .NotEmpty().WithMessage("'Second Name' must not be empty")
                .Matches("^[A-Za-z ]+$").WithMessage("'Middle Name' should contain only letters and spaces.")
                .MaximumLength(50).WithMessage("'Second Name' cannot be longer than 50 characters");

            RuleFor(n => n.FourthName)
                .NotEmpty().WithMessage("'Third Name' must not be empty")
                .Matches("^[A-Za-z ]+$").WithMessage("'Fourth Name' should contain only letters and spaces.")
                .MaximumLength(50).WithMessage("'Third Name' cannot be longer than 50 characters");

            RuleFor(n => n.LastName)
                .NotEmpty().WithMessage("'Last Name' must not be empty")
                .Matches("^[A-Za-z ]+$").WithMessage("'Last Name' should contain only letters and spaces.")
                .MaximumLength(50).WithMessage("'Last Name' cannot be longer than 50 characters");

            RuleFor(p => p.PhoneNumber)
                .NotEmpty().WithMessage("'Phone Number' must not be empty")
                .MinimumLength(10).WithMessage("'Phone Number' cannot be less than 10 characters.")
                .MaximumLength(20).WithMessage("'Phone Number' cannot exceed 20 characters.")
                .Matches(new Regex(@"^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$"))
                .WithMessage("'PhoneNumber' is not valid");

            RuleFor(n => n.Email)
                .NotEmpty().WithMessage("'Email Address' must not be empty")
                .EmailAddress().WithMessage("Invalid 'Email Address' format")
                .MaximumLength(50).WithMessage("'Email Address' cannot be longer than 50 characters")
                 .Matches(_emailPattern).WithMessage("Invalid email address format. Please enter a valid 'gmail', 'yahoo', or 'techprocess' email address ending with '.com' or '.net'"); 

            RuleFor(n => n.PositionId)
                .NotNull().WithMessage("Please select a valid 'Position'")
                .GreaterThan(0).WithMessage("Please select a valid 'Position'");

            RuleFor(n => n.LevelId)
                .NotNull().WithMessage("Please select a valid 'Level of Experience'")
                .GreaterThan(0).WithMessage("Please select a valid 'Level of Experience'");

            RuleFor(n => n.TeamCountryId)
                .NotNull().WithMessage("Please select a valid 'Country Team'")
                .GreaterThan(0).WithMessage("Please select a valid 'Country Team'");

            RuleFor(n => n.Gender)
                .NotEmpty().WithMessage("Please select a valid 'Gender'");

            RuleFor(n => n.Photo)
                .NotEmpty().WithMessage("The 'Profile Picture' must not be empty")
                .Must(photo => photo == null || IsValidImage(photo))
                .WithMessage("Please upload a valid image file. Allowed formats: .jpg, .jpeg, .png");

            RuleFor(n => n.Address)
                .NotEmpty().WithMessage("The 'Address' must not be empty")
                .MaximumLength(50).WithMessage("'Address' cannot be longer than 50 characters");
        }
        private bool IsValidImage(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            return _permittedExtensions.Contains(extension);
        }
    }
}
