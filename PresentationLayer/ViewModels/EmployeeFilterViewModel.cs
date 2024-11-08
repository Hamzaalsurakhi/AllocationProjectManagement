using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.ViewModels
{
    public class EmployeeFilterViewModel
    {
        public int? SelectedLevelId { get; set; }
        public int? SelectedPositionId { get; set; }
        public string? SearchString { get; set; }

        public IEnumerable<SelectListItem>? Levels { get; set; }
        public IEnumerable<SelectListItem>? Positions { get; set; }
    }
}
