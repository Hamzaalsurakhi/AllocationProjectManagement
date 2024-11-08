using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PresentationLayer.Validations.UserValidation
    {
    public class PhoneNumberValidation:ValidationAttribute
        {
        private static readonly Regex PhoneNumberRegex = new Regex(
           @"^\+?\d{1,4}[\s.-]?\(?\d{1,5}\)?[\s.-]?\d{1,4}[\s.-]?\d{1,9}$",
           RegexOptions.Compiled);

        protected override ValidationResult? IsValid( object? value, ValidationContext validationContext )
            {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                return new ValidationResult("Phone number is required.");
                }

            var phoneNumber = value.ToString();
            if (PhoneNumberRegex.IsMatch(phoneNumber))
                {
                return ValidationResult.Success;
                }
            else
                {
                return new ValidationResult("Invalid phone number format. Please use a valid phone number.");
                }
            }
        }
    }
