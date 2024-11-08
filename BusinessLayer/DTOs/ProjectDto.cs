using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifyOn { get; set; }
        public int? ModifyBy { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int StatusId { get; set; }
        public List<AllocationDto> Allocations { get; set; } = new List<AllocationDto>();


    }
}
