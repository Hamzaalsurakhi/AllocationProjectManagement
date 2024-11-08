namespace BusinessLayer.DTOs
{
    public class AllocationHistoryDto
    {
        public int EmployeeId { get; set; }
        public int AllocationId { get; set; }
        public string ProjectName { get; set; }
        public string EmployeeName { get; set; }
        public int AllocationPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PositionName { get; set; }
        public string levelName { get; set; }
    }
}
