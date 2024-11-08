using AspNetCore.Reporting;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class ReportController : Controller
    {

        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IEmployeeService _employeeService;
        private readonly IProjectService _projectService;
        private readonly IAllocationService _allocationService;

        public ReportController(IWebHostEnvironment webHostEnvironment, IEmployeeService employeeService, IProjectService projectService, IAllocationService allocationService)
        {
            this.webHostEnvironment = webHostEnvironment;
            _employeeService = employeeService;
            _projectService = projectService;
            _allocationService = allocationService;
        }
        public IActionResult ProjectReport()
        {
            return View();
        }
        public async Task<IActionResult> ProjectsFilterReport(DateTime? startDate, DateTime? endDate, string search, int page = 1, int pageSize = 10)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.MinValue;
            }
            if (!endDate.HasValue)
            {
                endDate = DateTime.MaxValue;
            }

            if (startDate > endDate)
            {
                ModelState.AddModelError(string.Empty, "Start Date must be less than or equal to End Date.");
                return Json(new { errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray() });
            }

            var data = await _projectService.GetProjectsFilteredReport(startDate.Value, endDate.Value, search, page, pageSize);

            var path = Path.Combine(webHostEnvironment.WebRootPath, "RDLCReport/RDLCProjectReport/ProjectReport.rdlc");
            LocalReport report = new LocalReport(path);
            report.AddDataSource("ProjectDataSet", data);
            var reportBytes = report.Execute(RenderType.Pdf, 1, null, "");

            return File(reportBytes.MainStream, "application/pdf");
        }

        public IActionResult EmployeeReport()
        {
            return View();
        }

        public async Task<IActionResult> EmployeeFilterReport(DateTime? startDate, DateTime? endDate, string? search, int page = 1, int pageSize = 10)
        {
            // Default to a large date range if no dates are provided
            if (!startDate.HasValue)
            {
                startDate = DateTime.MinValue;
            }
            if (!endDate.HasValue)
            {
                endDate = DateTime.MaxValue;
            }

            if (startDate > endDate)
            {
                ModelState.AddModelError(string.Empty, "Start Date must be less than or equal to End Date.");
                return Json(new { errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray() });
            }

            var data = await _employeeService.GetAllEmployeeFilteredReport(startDate.Value, endDate.Value, search, page, pageSize);

            var path = Path.Combine(webHostEnvironment.WebRootPath, "RDLCReport/RDLCEmployeeReport/EmployeeReport.rdlc");
            LocalReport report = new LocalReport(path);
            report.AddDataSource("EmployeeDataSet", data);
            var reportBytes = report.Execute(RenderType.Pdf, 1, null, "");

            return File(reportBytes.MainStream, "application/pdf");
        }
        //
        public IActionResult AllocationReport()
        {
            return View();
        }
        public async Task<IActionResult> AllocationFilterReport(DateTime? startDate, DateTime? endDate, string search, int page = 1, int pageSize = 10)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.MinValue;
            }
            if (!endDate.HasValue)
            {
                endDate = DateTime.MaxValue;
            }

            if (startDate > endDate)
            {
                ModelState.AddModelError(string.Empty, "Start Date must be less than or equal to End Date.");
                return Json(new { errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray() });
            }

            var data = await _allocationService.GetAllocationsFilteredReport(startDate.Value, endDate.Value, search, page, pageSize);

            var path = Path.Combine(webHostEnvironment.WebRootPath, "RDLCReport/RDLCAllocationReport/AllocationReport.rdlc");
            LocalReport report = new LocalReport(path);
            report.AddDataSource("AllocationDataSet", data);
            var reportBytes = report.Execute(RenderType.Pdf, 1, null, "");

            return File(reportBytes.MainStream, "application/pdf");
        }
    }
}
