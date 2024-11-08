using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Employee : BaseEntity
    {
        [Key]
        public int EmployeeId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(50)]
        public string MidName { get; set; } = string.Empty;
        [StringLength(50)]
        public string FourthName { get; set; } = string.Empty;
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        public string? PhotoURL { get; set; }
        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;
        public bool IsBench { get; set; } = true;

        public int PositionId { get; set; }
        public LookUp Position { get; set; }
        public int TotalAllocatedPercentage { get; set; }
        public int LevelId { get; set; }
        public LookUp Level { get; set; }

        public int TeamCountryId { get; set; }
        public LookUp TeamCountry { get; set; }

        public string Gender { get; set; }

        public ICollection<Allocation>? ProjectAllocation { get; set; }


    }
}
