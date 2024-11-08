using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Allocation : BaseEntity
    {
        [Key]
        public int AllocationId { get; set; }
        public int projectId { get; set; }
        public Project Project { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int AllocationPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? LevelId { get; set; }
        public LookUp Level { get; set; }
        public int? PositionId { get; set; }
        public LookUp Position { get; set; }
    }
}
