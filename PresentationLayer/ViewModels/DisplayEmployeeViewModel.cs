namespace PresentationLayer.ViewModels
{
    public class DisplayEmployeeViewModel
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
        public int TotalAllocatedPercentage { get; set; } = 0;
        public int LevelId { get; set; }
        public int TeamCountryId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Gender { get; set; }
        public string? PositionName { get; set; }
        public string? LevelName { get; set; }
        public string? TeamCountry { get; set; }
        //public List<string>? ProjectNames { get; set; }
        public IEnumerable<AllocationViewModel>? ActiveAllocations { get; set; }
    }
}
