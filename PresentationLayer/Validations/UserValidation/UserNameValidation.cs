using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PresentationLayer.Validations.UserValidation
    {
    public class UserNameValidation: ValidationAttribute
        {
        protected override ValidationResult? IsValid( object? value, ValidationContext validationContext )
            {
            var username = value?.ToString();

            if (value == null || string.IsNullOrEmpty(username))
                {
                return new ValidationResult("Username is required.");
                }
            string regex = @"^[a-z][a-z0-9._-]{3,30}$";
            if (Regex.IsMatch(username, regex))
                {
                return ValidationResult.Success;
                }
            else
                {
                return new ValidationResult("Username must start with a letter and be 3-20 characters long, containing only letters, numbers, underscores, or dots.");
                }
            }
        }
    }
