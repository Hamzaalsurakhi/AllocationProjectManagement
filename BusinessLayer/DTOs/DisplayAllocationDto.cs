namespace BusinessLayer.DTOs
{
    public class DisplayAllocationDto
    {
        public int AllocationId { get; set; }
        public string ProjectName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AllocationPercentage { get; set; }
        public int? PositionId { get; set; }
        public int? LevelId { get; set; }
    }
}
