using PresentationLayer.Validations.UserValidation;

namespace PresentationLayer.ViewModel
{
    public class LoginViewModel
    {
        [UserNameValidation]
        public string UserName { get; set; }
        [PasswordValidation]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
