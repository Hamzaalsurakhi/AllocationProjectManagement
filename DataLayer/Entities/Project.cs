using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Project : BaseEntity
    {
        [Key]
        public int ProjectId { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } = DateTime.MinValue;
        public ICollection<Allocation> allocations { get; set; }
        public LookUp Status { get; set; }
        public int StatusId { get; set; }


    }
}
