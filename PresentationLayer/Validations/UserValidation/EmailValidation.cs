using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PresentationLayer.Validations.UserValidation
    {
    public class EmailValidation:ValidationAttribute
        { 
        protected override ValidationResult? IsValid( object? value, ValidationContext validationContext )
            {
            var Email = value?.ToString();

            if (value == null || string.IsNullOrEmpty(Email))
                {
                return new ValidationResult("Email address is required.");
                }

            var email = value?.ToString()?.ToLower();
            string regexPattern = @"^(?=.*[a-z])[a-z\d\._-]+@(gmail|yahoo|nextwo)\.(com|net)$";

            if (Regex.IsMatch(email, regexPattern))
                {
                return ValidationResult.Success;
                }
            else
                {
                return new ValidationResult("Invalid email address format. Please enter a valid 'gmail' or 'yahoo' or  'nextwo' email address ending with '.com' or '.net'");
                }
            }
        }
    }
