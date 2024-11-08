namespace BusinessLayer.DTOs
{
    public class ResetPasswordDto
    {

        public string CurrentPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
