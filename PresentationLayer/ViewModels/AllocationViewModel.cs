using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.ViewModels;

public class AllocationViewModel
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
    public IEnumerable<SelectListItem> Levels { get; set; }
    public IEnumerable<SelectListItem> Positions { get; set; }
    public IEnumerable<SelectListItem> Projects { get; set; }
}


