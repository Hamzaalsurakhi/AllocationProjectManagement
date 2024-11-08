using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.ViewModels
{
    public class LookUpViewModel
    {

        public IEnumerable<SelectListItem> Statuses { get; set; }
        public int? SelectedStatus { get; set; }
    }
}
