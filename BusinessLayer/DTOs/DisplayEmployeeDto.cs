namespace BusinessLayer.DTOs
{
    public class DisplayEmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MidName { get; set; } = string.Empty;
        public string FourthName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhotoURL { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsBench { get; set; } = true;
        public int PositionId { get; set; }
        public int TotalAllocatedPercentage { get; set; }
        
        public int LevelId { get; set; }
        public int TeamCountryId { get; set; }
        public bool IsDeleted { get; set; }
        public string Gender { get; set; } = null!;
        public string PositionName { get; set; }
        public string LevelName { get; set; }
        public string FullName { get; set; }

        public string TeamCountry { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? FormattedCreatedOn { get; set; }
        public IEnumerable<AllocationDto>? ActiveAllocations { get; set; }
    }
}
