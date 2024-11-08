using PresentationLayer.Validations.UserValidation;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ViewModels
{
    public class UserViewModel
    {

        public int? Id { get; set; }
        [Required(ErrorMessage = "The First Name field  is required.")]
        [StringLength(20, ErrorMessage = "First Name cannot be longer than 20 characters.")]
        [RegularExpression(@"^[a-zA-Z]+(?: [a-zA-Z]+)*$", ErrorMessage = "First Name can only contain letters and spaces, but cannot start or end with a space.")]
        public string Fname { get; set; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        [RegularExpression(@"^[a-zA-Z]+(?:[ -][a-zA-Z]+)*$", ErrorMessage = "Last Name can only contain letters, spaces, or hyphens, but cannot start or end with a space or hyphen.")]
        public string Lname { get; set; }
        [UserNameValidation]
        public string UserName { get; set; }
        [EmailValidation]
        public string Email { get; set; }
        [PhoneNumberValidation]
        public string PhoneNumber { get; set; }

        public string? PhotoUrl { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public DateTime? ModifyOn { get; set; } = DateTime.Now;

        public DateTime? CreatedOn { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression(@"^(male|female)$", ErrorMessage = "Gender must be 'male' or 'female'.")]
        public string Gender { get; set; }
    }
}
