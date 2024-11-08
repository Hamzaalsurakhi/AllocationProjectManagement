namespace PresentationLayer.ViewModels
{
    public class EmployeeWithTimeDifferenceViewModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoURL { get; set; }
        public string PositionName { get; set; }
        public string LevelName { get; set; }
        public TimeSpan? TimeDifference { get; set; }
    }
}
