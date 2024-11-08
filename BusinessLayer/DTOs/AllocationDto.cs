namespace BusinessLayer.DTOs;

public class AllocationDto
{
    public int AllocationId { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public int AllocationPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? LevelId { get; set; }
    public string LevelName { get; set; }
    public int? PositionId { get; set; }
    public string PositionName { get; set; }

}

public class EmployeeAllocationDto
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public int RemainAllocationPercentage { get; set; }
}
