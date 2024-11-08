using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.ViewModels;
using System.Linq.Dynamic.Core;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Project Delivery Manager")]
    public class ProjectController : Controller
    {


        private readonly IProjectService _projectService;
        private readonly ILookUpService _lookUpService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, ILookUpService lookUpService, IMapper mapper)
        {
            _projectService = projectService;
            _lookUpService = lookUpService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Project Delivery Manager")]
        public async Task<IActionResult> ShowProjects()
        {
            return View();
        }

        [Authorize(Roles = "Project Delivery Manager")]
        [HttpPost]
        public async Task<IActionResult> GetProjects()
        {
            var draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
            var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "0");
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];
            var searchValue = Request.Form["search[value]"];

            var (project, totalRecords) = await _projectService.GetProjectsTableAsync(start, length, searchValue, sortColumn, sortColumnDirection);

            var jsonData = new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = project
            };

            return Ok(jsonData);
        }

        [Authorize(Roles = "Project Delivery Manager")]
        public async Task<IActionResult> Details(int id)
        {
            var projectDto = await _projectService.GetProjectWithAllocationsAsync(id);
            var viewModel = _mapper.Map<DisplayProjectViewModel>(projectDto);
            return Json(viewModel);
        }

        [HttpGet]
        public IActionResult DetailsPartial(int id)
        {
            var project = _projectService.GetProjectWithAllocationsAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<DisplayProjectViewModel>(project);
            return PartialView("_ProjectDetailsPartial", viewModel);
        }


        [Authorize(Roles = "Project Delivery Manager")]
        public async Task<IActionResult> Create()
        {
         

            var viewModel = new ProjectViewModel();
            viewModel = await PopulateDropdownLists(viewModel);
            return View(viewModel);
        }

        [Authorize(Roles = "Project Delivery Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(ProjectViewModel projects)
        {
            if (ModelState.IsValid)
            {
                var projectDto = _mapper.Map<ProjectDTO>(projects);
                await _projectService.AddProjectAsync(projectDto);
                TempData["SuccessCreate"] = "Project created successfully!";
                return RedirectToAction("Create");
            }
            projects = await PopulateDropdownLists(projects);
            return View(projects);
        }




        [Authorize(Roles = "Project Delivery Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var projectDto = await _projectService.GetProjectByIdAsync(id);
            if (projectDto == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<ProjectViewModel>(projectDto);
            viewModel = await PopulateDropdownLists(viewModel);
            return View("Edit", viewModel);

        }

        [Authorize(Roles = "Project Delivery Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectViewModel projects)
        {
            if (ModelState.IsValid)
            {
                var projectDto = _mapper.Map<ProjectDTO>(projects);
                await _projectService.UpdateProjectAsync(projectDto);
                TempData["SuccessEdit"] = "Project created successfully!";
                return RedirectToAction("Edit");
            }
            projects = await PopulateDropdownLists(projects);
            return View(projects);
        }




        [HttpPost]
        [Authorize(Roles = "Project Delivery Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resualt = await _projectService.SoftDeleteProjectAsync(id);
            if (!resualt)
            {
                return NotFound();
            }
            TempData["SuccessDeleted"] = "Project deleted successfully!";

            return Json(new { hasAllocations = false, success = true });
        }

        [HttpPost]
        [Authorize(Roles = "Project Delivery Manager")]
        public async Task<IActionResult> CheckProjectAllocations(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            
            bool hasAllocations = project.Allocations != null && project.Allocations.Any();

            return Json(new { hasAllocations });
        }

        public async Task<IActionResult> DeletedProjects()
        {
            return View();
        }


        [Authorize(Roles = "Project Delivery Manager")]
        [HttpPost]
        public async Task<IActionResult> GetProjectsDeleted()
        {
            var draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
            var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "0");
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];
            var searchValue = Request.Form["search[value]"];

            var (project, totalRecords) = await _projectService.GetDeletedProjectsTableAsync(start, length, searchValue, sortColumn, sortColumnDirection);

            var jsonData = new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = project
            };

            return Ok(jsonData);
        }


        [HttpPost]
        public async Task<IActionResult> RestoreProject(int id)
        {

            var resualt = await _projectService.RestoreProjectAsync(id);
            if (!resualt)
            {
                return NotFound();
            }
            TempData["SuccessRestore"] = "Project Restore successfully!";
            return RedirectToAction("DeletedProjects");
        }


        private async Task<IEnumerable<SelectListItem>> GetDropDownItems(LookUpEnums.CategoryCode category, int? selectedValue = null)
        {
            var lookUps = await _lookUpService.GetLookUpsByCategoryAsync(category);
            return new SelectList(lookUps, "LookUpId", "NameEn", selectedValue);
        }


        private async Task<ProjectViewModel> PopulateDropdownLists(ProjectViewModel viewModel)
        {
            viewModel.Statuses = await GetDropDownItems(LookUpEnums.CategoryCode.Status, viewModel.StatusId);

            return viewModel;
        }
    }
}