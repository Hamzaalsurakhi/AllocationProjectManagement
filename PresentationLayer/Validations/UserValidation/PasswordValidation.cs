using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PresentationLayer.Validations.UserValidation
    {
    public class PasswordValidation: ValidationAttribute
        {
        protected override ValidationResult? IsValid( object? value, ValidationContext validationContext )
            {
            var password = value?.ToString();
            if (value == null || string.IsNullOrEmpty(password))
                {
                return new ValidationResult("Password is required.");
                }

            string regex = @"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}";
            if (Regex.IsMatch(password, regex))
                {
                return ValidationResult.Success;
                }
            else
                {
                return new ValidationResult("Password must be at least 8 characters long and contain at least one digit, one lowercase letter, one uppercase letter, and one non-alphanumeric character.");
                }
            }
        }
    }
    