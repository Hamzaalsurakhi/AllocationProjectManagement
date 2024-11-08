namespace BusinessLayer.DTOs
{
    public class AllocationDetailsDto
    {
        public int AllocationId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int AllocationPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string LevelNameEn { get; set; }
        public string PositionNameEn { get; set; }
    }
}
