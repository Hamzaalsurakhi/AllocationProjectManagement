namespace PresentationLayer.ViewModels
{
    public class DashboardViewModel
    {
        public int totalOfEmployee { get; set; }
        public int totalOfProject { get; set; }
        public int sharedResource { get; set; }
        public int NumberOfEmployeeInBench { get; set; }
        public Dictionary<string, int> ProjectStatusCounts { get; set; }
        public IEnumerable<EmployeeWithTimeDifferenceViewModel> EmployeesBecomeBench { get; set; }
    }
}
