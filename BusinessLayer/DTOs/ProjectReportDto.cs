namespace BusinessLayer.DTOs
{
    public class ProjectReportDto
    {
        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int StatusId { get; set; }

        public string? StatusName { get; set; }
        public string FormattedStartDate { get; set; }
        public string FormattedEndDate { get; set; }
    }
}
