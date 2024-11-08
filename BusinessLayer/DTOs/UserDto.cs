namespace BusinessLayer.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? ModifyOn { get; set; }

        public string PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public string Gender { get; set; }
    }
}
