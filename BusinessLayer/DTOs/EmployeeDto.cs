namespace BusinessLayer.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MidName { get; set; } = string.Empty;
        public string FourthName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhotoURL { get; set; }
        public required string PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsBench { get; set; } = true;
        public int PositionId { get; set; }
        public int TotalAllocatedPercentage { get; set; }
        public int LevelId { get; set; }
        public int TeamCountryId { get; set; }
        public bool IsDeleted { get; set; }
        public required string Gender { get; set; }

    }
}
