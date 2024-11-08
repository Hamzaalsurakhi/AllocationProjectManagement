namespace BusinessLayer.DTOs
{
    public class AllocationReportDto
    {
        public string ProjectName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string FormattedStartDate { get; set; }
        public string FormattedEndDate { get; set; }
    }
}
