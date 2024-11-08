using Microsoft.AspNetCore.Identity;

namespace DataLayer.Entities
{
    public class User : IdentityUser<int>
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifyOn { get; set; }
        public int? ModifyBy { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string Gender { get; set; }

    }
}
