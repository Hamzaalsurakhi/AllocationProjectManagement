using AutoMapper;
using Azure;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Attributes;
using PresentationLayer.ExtensionsServices;
using PresentationLayer.ViewModels;
using System.Linq.Dynamic.Core;
namespace PresentationLayer.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILookUpService _lookUpService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotificationService _notificationService;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILookUpService lookUpService, IWebHostEnvironment webHostEnvironment, INotificationService notificationService)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _lookUpService = lookUpService;
            _webHostEnvironment = webHostEnvironment;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new EmployeeViewModel();
            await PopulateDropdownLists(model);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
                var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
                var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "0");
                var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")].FirstOrDefault() ?? "0";
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "0";
                var searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "0";
                var positionId = Request.Form["positionId"].FirstOrDefault() ?? "0";
                var levelId = Request.Form["levelId"].FirstOrDefault() ?? "0";

                var (employees, totalRecords) = await _employeeService.GetEmployeesTableAsync(start, length, searchValue, sortColumn, sortColumnDirection, positionId, levelId);

                var jsonData = new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = employees
                };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetIsBenchEmployees()
        {
            try
            {
                var draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
                var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
                var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "0");
                var sortColumnIndex = int.Parse(Request.Form["order[0][column]"]);
                var sortColumnDirection = Request.Form["order[0][dir]"];
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var positionId = Request.Form["positionId"].FirstOrDefault();
                var levelId = Request.Form["levelId"].FirstOrDefault();

                var columnNames = new[]
                {
            "employeeId",
            "fullName",
            "email",
            "phoneNumber",
            "positionName",
            "levelName",
            "teamCountry"
        };

                var sortColumn = columnNames.ElementAtOrDefault(sortColumnIndex);

                // Get filtered and paginated IsBench employees
                var (employees, totalRecords) = await _employeeService.GetIsBenchEmployeesTableAsync(
                    start, length, searchValue, sortColumn, sortColumnDirection, positionId, levelId
                );

                var jsonData = new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = employees
                };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }





        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var employeeViewModel = _mapper.Map<DisplayEmployeeViewModel>(employee);
            return PartialView("_detailEmployeePartial", employeeViewModel);
        }






        [Authorize(Roles = "HR Officer")]
        public async Task<IActionResult> Add()
        {
            var viewModel = new EmployeeViewModel();
            viewModel = await PopulateDropdownLists(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "HR Officer")]
        public async Task<IActionResult> Add(EmployeeViewModel employees)
        {
            var Emp = await _employeeService.GetEmployeeByEmailAsync(employees.Email);
            if (Emp != null)
            {
                ModelState.AddModelError(nameof(employees.Email), "Email is already used!");
            }
            if (ModelState.IsValid)
            {
                string? photoUrl = null;
                if (employees.Photo != null && employees.Photo.Length > 0)
                {
                    photoUrl = await FileExtensions.ConvertFileToStringAsync(employees.Photo, _webHostEnvironment);
                }
                var employeeDto = _mapper.Map<EmployeeDto>(employees);
                employeeDto.PhotoURL = photoUrl;
                if (ModelState.ErrorCount > 0)
                {
                    return View(employees);
                }
                else
                {
                    await _employeeService.AddEmployeeAsync(employeeDto);
                }
                await _notificationService.CreateNotificationAsync(
                    "Bench Status",
                    $"Employee {employeeDto.FirstName} {employeeDto.MidName} {employeeDto.LastName} is on the bench."
                );

                TempData["SuccessCreateEmp"] = "Employee created successfully!";
                return RedirectToAction("Add");
            }

            employees.Photo = employees.Photo;
            employees = await PopulateDropdownLists(employees);
            return View(employees);
        }

        [Authorize(Roles = "HR Officer")]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);
            employeeViewModel = await PopulateDropdownLists(employeeViewModel);

            if (TempData["SuccessEditEmp"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessEditEmp"].ToString();
            }
            return View("Edit", employeeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR Officer")]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeViewModel)
        {
            var currentEmployee = await _employeeService.GetEmployeeByEmailAsync(employeeViewModel.Email);
            if (currentEmployee != null && currentEmployee.EmployeeId != employeeViewModel.EmployeeId)
            {
                ModelState.AddModelError(nameof(employeeViewModel.Email), "Email is already used!");
            }
            ModelState.Remove("Photo");
            if (ModelState.IsValid)
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employeeViewModel);
                if (employeeViewModel.Photo != null && employeeViewModel.Photo.Length > 0)
                {
                    employeeDto.PhotoURL = await FileExtensions.ConvertFileToStringAsync(employeeViewModel.Photo, _webHostEnvironment);

                    if (!string.IsNullOrEmpty(employeeViewModel.PhotoURL))
                    {
                        FileExtensions.DeleteFileFromFileFolder(employeeViewModel.PhotoURL, _webHostEnvironment);
                    }
                }
                else
                {
                    employeeDto.PhotoURL = employeeViewModel.PhotoURL;
                }
                if (ModelState.ErrorCount > 0)
                {
                    return View(employeeViewModel);
                }
                await _employeeService.UpdateEmployeeAsync(employeeDto);
                TempData["SuccessEditEmp"] = "Employee Edited successfully!";
                return RedirectToAction("Edit", new { id = employeeViewModel.EmployeeId });
            }
            employeeViewModel = await PopulateDropdownLists(employeeViewModel);

            return View("Edit", employeeViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "HR Officer")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                var result = await _employeeService.SoftDeleteEmployeeAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                TempData["SuccessDeleteEmp"] = "Employee deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { hasAllocations = true, message = ex.Message });
            }
        }


        [Authorize]
        public IActionResult DeletedEmployees()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetDeletedEmployees()
        {
            var draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
            var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "0");
            var sortColumnIndex = int.Parse(Request.Form["order[0][column]"].FirstOrDefault() ?? "0");
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault()?? "0";
            var searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "0";
            var positionId = Request.Form["positionId"].FirstOrDefault() ?? "0";
            var levelId = Request.Form["levelId"].FirstOrDefault() ?? "0";
            var columnNames = new[]
            {
                "firstName",
                "lastName",
                "email",
                "phoneNumber",
                "positionName",
                "levelName",
                "totalAllocatedPercentage"
            };

            var sortColumn = columnNames.ElementAtOrDefault(sortColumnIndex);

            var (employees, totalRecords) = await _employeeService.GetDeletedEmployeesTableAsync(start, length, searchValue, sortColumn, sortColumnDirection);

            var jsonData = new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = employees
            };

            return Ok(jsonData);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RestoreEmployee(int id)
        {

            var resualt = await _employeeService.RestoreEmployeeAsync(id);
            if (!resualt)
            {
                return NotFound();
            }
            TempData["SuccessRestoreEmp"] = "Employee Restore successfully!";
            return RedirectToAction("DeletedEmployees");
        }


        private async Task<IEnumerable<SelectListItem>> GetDropDownItems(LookUpEnums.CategoryCode category, int? selectedValue = null)
        {
            var lookUps = await _lookUpService.GetLookUpsByCategoryAsync(category);
            return new SelectList(lookUps, "LookUpId", "NameEn", selectedValue);
        }


        private async Task<EmployeeViewModel> PopulateDropdownLists(EmployeeViewModel viewModel)
        {
            viewModel.Levels = await GetDropDownItems(LookUpEnums.CategoryCode.Level, viewModel.LevelId);
            viewModel.Positions = await GetDropDownItems(LookUpEnums.CategoryCode.Position, viewModel.PositionId);
            viewModel.TeamCountries = await GetDropDownItems(LookUpEnums.CategoryCode.TeamCountry, viewModel.TeamCountryId);
            return viewModel;
        }

    }
}
