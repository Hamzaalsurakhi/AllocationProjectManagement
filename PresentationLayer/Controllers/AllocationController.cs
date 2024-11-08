using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataLayer.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.ViewModels;
using System.Linq.Dynamic.Core;



namespace PresentationLayer.Controllers;
[Authorize(Roles = "Project Delivery Manager")]
public class AllocationController : Controller
{
    private readonly IAllocationService _allocationService;
    private readonly IMapper _mapper;
    private readonly ILookUpService _lookUpService;
    private readonly IProjectService _projectService;
    private readonly IEmployeeService _employeeService;

    public AllocationController(IAllocationService allocationService, IMapper mapper, ILookUpService lookUpService, IProjectService projectService, IEmployeeService employeeService)
    {
        _allocationService = allocationService;
        _mapper = mapper;
        _lookUpService = lookUpService;
        _projectService = projectService;
        _employeeService = employeeService;
    }

    [HttpGet]
    [Authorize(Roles = "Project Delivery Manager")]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    [Authorize(Roles = "Project Delivery Manager")]
    public async Task<IActionResult> GetAllAlocation()
    {
        var draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
        var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
        var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "0");
        var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")].FirstOrDefault() ?? "0";
        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "0";
        var searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "0";

        var (allocations, totalRecords) = await _allocationService.GetAllocationTableAsync(start, length, searchValue, sortColumn, sortColumnDirection);

        var jsonData = new
        {
            draw = draw,
            recordsTotal = totalRecords,
            recordsFiltered = totalRecords,
            data = allocations
        };

        return Ok(jsonData);
    }

    [HttpGet]
    [Authorize(Roles = "Project Delivery Manager")]
    public async Task<IActionResult> Create()
    {
        var viewModel = new CreateAllocationViewModel
        {
            Employees = await GetEmployeesAsync(null, null, null, null),
            Projects = await GetProjectsAsync(),
        };
        await PopulateDropdownLists(viewModel);

        return View(viewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Project Delivery Manager")]
    public async Task<IActionResult> Create(CreateAllocationViewModel viewModel)
    {

        if (!ModelState.IsValid)
        {
            viewModel.Employees = await GetEmployeesAsync(viewModel.LevelId, viewModel.PositionId, viewModel.AllocationPercentage, viewModel.ProjectId);
            viewModel.Projects = await GetProjectsAsync();
            await PopulateDropdownLists(viewModel);
            return View(viewModel);
        }
        var project = await _projectService.GetProjectByIdAsync(viewModel.ProjectId);
        if (project.EndDate <= viewModel.EndDate)
        {
            ModelState.AddModelError(nameof(viewModel.EndDate), $"The EndDate must be less than or equal EndDate of Project (Project ends on {project.EndDate.ToString()})");
        }
        if (project.StartDate > viewModel.StartDate)
        {
            ModelState.AddModelError(nameof(viewModel.StartDate), $"The StartDate must be greater than or equal startDate of Project (Project starts on {project.StartDate.ToString()})");
        }
        if (ModelState.ErrorCount > 0)
        {
            viewModel.Employees = await GetEmployeesAsync(viewModel.LevelId, viewModel.PositionId, viewModel.AllocationPercentage, viewModel.ProjectId);
            viewModel.Projects = await GetProjectsAsync();
            await PopulateDropdownLists(viewModel);
            return View(viewModel);
        }
        var allocationDto = _mapper.Map<AddAllocationDto>(viewModel);
        try
        {
            await _allocationService.CreateAllocationAsync(allocationDto);
            TempData["SuccessCreate"] = "Allocation created successfully!";
            return RedirectToAction("Create");
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError("", error.ErrorMessage);
            }
            viewModel.Employees = await GetEmployeesAsync(viewModel.LevelId, viewModel.PositionId, viewModel.AllocationPercentage, viewModel.ProjectId);
            viewModel.Projects = await GetProjectsAsync();
            await PopulateDropdownLists(viewModel);
            return View(viewModel);
        }
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var allocation = await _allocationService.GetAllocationByIdAsync(id);
        if (allocation == null)
        {
            return NotFound();
        }
        var viewModel = _mapper.Map<EditAllocationViewModel>(allocation);
        viewModel.Projects = await GetFilteredProjectAsync(viewModel.EmployeeId, viewModel.AllocationId);


        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditAllocationViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.Projects = await GetFilteredProjectAsync(viewModel.EmployeeId, viewModel.AllocationId);

            return View(viewModel);
        }
        var project = await _projectService.GetProjectByIdAsync(viewModel.projectId);
        if (project.EndDate <= viewModel.EndDate)
        {
            ModelState.AddModelError(nameof(viewModel.EndDate), $"The EndDate must be less than or equal EndDate of Project (Project ends on {project.EndDate.ToString()})");
        }
        if (project.StartDate >= viewModel.StartDate)
        {
            ModelState.AddModelError(nameof(viewModel.StartDate), $"The StartDate must be greater than or equal startDate of Project (Project Starts on {project.StartDate.ToString()})");
        }
        if (ModelState.ErrorCount > 0)
        {
            viewModel.Projects = await GetFilteredProjectAsync(viewModel.EmployeeId, viewModel.AllocationId);
            return View(viewModel);
        }
        var allocationDto = _mapper.Map<EditAllocationDto>(viewModel);

        try
        {
            await _allocationService.EditAllocationAsync(allocationDto);
            TempData["editSuccess"] = "Edit allocation Done Successfully";
            return RedirectToAction("Edit");
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError("", error.ErrorMessage);
            }
            viewModel.Projects = await GetFilteredProjectAsync(viewModel.EmployeeId, viewModel.AllocationId);
            return View(viewModel);
        }
    }


    private async Task<IEnumerable<SelectListItem>> GetFilteredProjectAsync(int? employeeId, int? aloocationId)
    {
        var projects = await _allocationService.GetFilteredProjectsAsync(employeeId.GetValueOrDefault(), aloocationId.GetValueOrDefault());
        return projects.Select(e => new SelectListItem
        {
            Value = e.ProjectId.ToString(),
            Text = e.Name
        }).ToList();
    }


    [HttpPost]
    [Authorize(Roles = "Project Delivery Manager")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var resualt = await _allocationService.SoftDeleteAllocationAsync(id);
        if (!resualt)
        {
            return NotFound();
        }
        TempData["SuccessDeleteAlloc"] = "Allocation deleted successfully!";
        return RedirectToAction("Index");
    }


    private async Task<IEnumerable<SelectListItem>> GetEmployeesAsync(int? levelId, int? positionId, int? allocationPercentage, int? projectId)
    {

        var employees = await _allocationService.GetEmployeesForAllocationAsync(levelId.GetValueOrDefault(), positionId.GetValueOrDefault(), allocationPercentage.GetValueOrDefault(), projectId.GetValueOrDefault());
        return employees.Select(e => new SelectListItem
        {
            Value = e.EmployeeId.ToString(),
            Text = e.EmployeeName
        }).ToList();
    }

    private async Task<IEnumerable<SelectListItem>> GetProjectsAsync()
    {
        // Fetch projects from the service and map them to SelectListItem
        var projects = await _projectService.GetAllProjectsAsync();
        return projects.Select(p => new SelectListItem
        {
            Value = p.ProjectId.ToString(),
            Text = p.Name
        }).ToList();
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees(int? levelId, int? positionId, int? allocationPercentage, int? projectId)
    {
        var employees = await GetEmployeesAsync(levelId, positionId, allocationPercentage, projectId);
        return PartialView("_EmployeeList", employees);
    }

    private async Task<IEnumerable<SelectListItem>> GetDropDownItems(LookUpEnums.CategoryCode category, int? selectedValue = null)
    {
        var lookUps = await _lookUpService.GetLookUpsByCategoryAsync(category);
        return new SelectList(lookUps, "LookUpId", "NameEn", selectedValue);
    }


    private async Task<CreateAllocationViewModel> PopulateDropdownLists(CreateAllocationViewModel viewModel)
    {
        viewModel.Levels = await GetDropDownItems(LookUpEnums.CategoryCode.Level, viewModel.LevelId);
        viewModel.Positions = await GetDropDownItems(LookUpEnums.CategoryCode.Position, viewModel.PositionId);

        return viewModel;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeeDistributionData()
    {
        var data = await _allocationService.GetEmployeeDistributionByProjectAsync();
        return Json(data);
    }


    [HttpGet]
    public IActionResult AllocationHistory()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AllocationHistory(int id)
    {

        var draw = Request.Form["draw"].FirstOrDefault() ?? "0";
        var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
        var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "0");
        var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")].FirstOrDefault() ?? "0";
        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "0";
        var searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "0";

        var allocationHistory = await _allocationService.GetEmployeeAllocationHistoryAsync(id, searchValue, sortColumn, sortColumnDirection);

        var pagedHistory = allocationHistory.Skip(start).Take(length);
        var totalRecords = allocationHistory.Count();
        var jsonData = new
        {
            draw = draw,
            recordsTotal = totalRecords,
            recordsFiltered = totalRecords,
            data = pagedHistory
        };

        return Ok(jsonData);
    }


    [HttpGet]
    public async Task<IActionResult> CreateBenchAllocation(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<CreateAllocationViewModel>(employee);
        viewModel.Projects = await GetProjectsAsync();

        return View(viewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBenchAllocation(CreateAllocationViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var employeeId = viewModel.SelectedEmployeeIds.FirstOrDefault();
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            viewModel = _mapper.Map<CreateAllocationViewModel>(employee);
            viewModel.Projects = await GetProjectsAsync();

            return View(viewModel);
        }
        var project = await _projectService.GetProjectByIdAsync(viewModel.ProjectId);
        if (project.EndDate <= viewModel.EndDate)
        {
            ModelState.AddModelError(nameof(viewModel.EndDate), $"The EndDate must be less than or equal EndDate of Project (Project ends on {project.EndDate.ToString()})");
        }
        if (project.StartDate > viewModel.StartDate)
        {
            ModelState.AddModelError(nameof(viewModel.StartDate), $"The StartDate must be greater than or equal startDate of Project (Project starts on {project.StartDate.ToString()})");
        }
        if (ModelState.ErrorCount > 0)
        {
            viewModel.Employees = await GetEmployeesAsync(viewModel.LevelId, viewModel.PositionId, viewModel.AllocationPercentage, viewModel.ProjectId);
            viewModel.Projects = await GetProjectsAsync();
            await PopulateDropdownLists(viewModel);
            return View(viewModel);
        }
        var allocationDto = _mapper.Map<AddAllocationDto>(viewModel);

        try
        {
            await _allocationService.CreateAllocationAsync(allocationDto);
            TempData["SuccessCreateAllocation"] = $"Allocation created successfully for {viewModel.EmployeeName}!";
            return RedirectToAction("Bench", "Home");
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError("", error.ErrorMessage);
            }

            var employeeId = viewModel.SelectedEmployeeIds.FirstOrDefault();
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee != null)
            {
                viewModel = _mapper.Map<CreateAllocationViewModel>(employee);
                viewModel.Projects = await GetProjectsAsync();
            }

            return View(viewModel);
        }
    }
}
