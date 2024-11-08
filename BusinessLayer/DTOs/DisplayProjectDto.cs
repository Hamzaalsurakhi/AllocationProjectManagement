namespace BusinessLayer.DTOs
{
    public class DisplayProjectDto
    {

        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int StatusId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifyOn { get; set; }
        public int? ModifyBy { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string? StatusName { get; set; }

        public List<AllocationDetailsDto> Allocations { get; set; }

    }
}
