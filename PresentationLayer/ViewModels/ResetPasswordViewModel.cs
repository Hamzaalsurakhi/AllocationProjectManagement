using PresentationLayer.Validations.UserValidation;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ViewModels
{
    public class ResetPasswordViewModel
    {
        [PasswordValidation]
        public string CurrentPassword { get; set; }

        [PasswordValidation]
        public string NewPassword { get; set; }
        [PasswordValidation]
        [Compare("NewPassword", ErrorMessage = "Confirm Password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
