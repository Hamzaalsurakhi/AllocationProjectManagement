namespace PresentationLayer.ViewModels
{
    public class DisplayAllocationViewModel
    {
        public int AllocationId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } // Optionally join first and last names
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LevelId { get; set; } // Added LevelId
        public int PositionId { get; set; } // Added PositionId
    }
}
