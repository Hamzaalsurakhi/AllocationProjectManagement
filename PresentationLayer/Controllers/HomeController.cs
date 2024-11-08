using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using BusinessLayer.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PresentationLayer.ExtensionsServices;
using PresentationLayer.ViewModels;
using System.Linq.Dynamic.Core;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IProjectService _projectService;
        private readonly IAllocationService _allocationService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public HomeController(INotificationService notificationService,
                              IWebHostEnvironment webHostEnvironment,
                              IMapper mapper,
                              IUserService userService,
                              ILogger<HomeController> logger,
                              IEmployeeService employeeService,
                              IProjectService projectService, IAllocationService allocationService, IHubContext<NotificationHub> hubContext)
        {
            _notificationService = notificationService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
            _employeeService = employeeService;
            _projectService = projectService;
            _allocationService = allocationService;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<EmployeeWithTimeDifferenceViewModel>> EmployeesBecomeBench()
        {

            var resualt = await _employeeService.EmployeesBecomeBench();
            var EmployeeWithTimeDifferenceViewModel = _mapper.Map<IEnumerable<EmployeeWithTimeDifferenceViewModel>>(resualt);
            return EmployeeWithTimeDifferenceViewModel;
        }

        [Authorize(Roles = "Project Delivery Manager")]
        public async Task<IActionResult> Dashboard()
        {
            await _allocationService.UpdateEmployeesStatusAsync();
            var totalOfEmployee = await _employeeService.GetAllEmployeesAsync();
            var totalOfProject = await _projectService.GetAllProjectsAsync();
            var projectStatusCounts = await _projectService.GetProjectStatusCountsAsync();
            try
            {

                var dashboardViewModel = new DashboardViewModel
                {
                    totalOfEmployee = totalOfEmployee.Count(),
                    EmployeesBecomeBench = await EmployeesBecomeBench(),
                    totalOfProject = totalOfProject.Count(),
                    NumberOfEmployeeInBench = await _employeeService.NumberOfEmployeeInBench(),
                    sharedResource = await _allocationService.TotalOfSharedResource(),
                    ProjectStatusCounts = projectStatusCounts.ToDictionary(k => k.Status, v => v.Count)
                };

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the dashboard.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetFilteredProjects([FromBody] DateRangeFilter filter)
        {
            try
            {
                var startDate = DateTime.Parse(filter.StartDate);
                var endDate = DateTime.Parse(filter.EndDate);

                var projects = await _projectService.GetProjectsByDateRangeAsync(startDate, endDate);

                var yearCounts = new Dictionary<int, int>();

                foreach (var project in projects)
                {
                    int endYear = project.EndDate.Value.Year;

                    if (endYear >= startDate.Year && endYear <= endDate.Year)
                    {
                        if (!yearCounts.ContainsKey(endYear))
                        {
                            yearCounts[endYear] = 0;
                        }
                        yearCounts[endYear]++;
                    }
                }

                var labels = yearCounts.Keys.OrderBy(y => y).Select(y => y.ToString()).ToList();
                var values = labels.Select(label => yearCounts[int.Parse(label)]).ToList();

                var result = new
                {
                    labels = labels,
                    values = values
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting filtered projects.");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Project Delivery Manager")]
        public IActionResult Bench()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var userDto = await _userService.GetUserByNameAsync(User.Identity.Name);
                var userViewModel = _mapper.Map<UserViewModel>(userDto);
                return View(userViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(string currentUserName, UserViewModel model)
        {
            var currentUser = await _userService.GetUserByNameAsync(currentUserName);

            if (!ModelState.IsValid)
            {
                model.PhotoUrl = currentUser.PhotoUrl;
                return View(model);
            }

            try
            {
                if (currentUser == null)
                {
                    return NotFound("User not found.");
                }

                var existingEmailUser = await _userService.CheckEmailIsFindAsync(model.Email);
                if (existingEmailUser != null && existingEmailUser.Id != currentUser.Id)
                {
                    ModelState.AddModelError(nameof(model.Email), "Email is already in use!");
                }

                var existingUserNameUser = await _userService.CheckUserNameIsFindAsync(model.UserName);
                if (existingUserNameUser != null && existingUserNameUser.Id != currentUser.Id)
                {
                    ModelState.AddModelError(nameof(model.UserName), "Username is already in use!");
                }

                if (!ModelState.IsValid)
                {
                    model.PhotoUrl = currentUser.PhotoUrl;
                    return View(model);
                }

                if (currentUser.UserName != model.UserName)
                {
                    await _userService.SetUserNameAsync(currentUser, model.UserName);
                }

                var updatedUserDto = _mapper.Map<UserDto>(model);

                if (model.ProfileImage != null)
                {
                    if (!string.IsNullOrEmpty(currentUser.PhotoUrl))
                    {
                        FileExtensions.DeleteFileFromFileFolder(currentUser.PhotoUrl, _webHostEnvironment);
                    }

                    updatedUserDto.PhotoUrl = await FileExtensions.ConvertFileToStringAsync(model.ProfileImage, _webHostEnvironment);
                }
                else
                {
                    updatedUserDto.PhotoUrl = currentUser.PhotoUrl;
                }

                var result = await _userService.UpdateUserAsync(updatedUserDto);

                if (result.Succeeded)
                {
                    TempData["UpdateAlert"] = "User Information is Updated";
                    return RedirectToAction("Profile", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating profile.");
                return StatusCode(500, "Internal server error");
            }

            var userViewModel = _mapper.Map<UserViewModel>(currentUser);
            return View(userViewModel);
        }

        [Authorize(Roles = "Project Delivery Manager")]
        public async Task<IActionResult> Notifications()
        {

            var notifications = await _notificationService.GetAllNotificationNotDeleted();
            var notificationViewModel = _mapper.Map<IEnumerable<NotificationViewModel>>(notifications);
            return View(notificationViewModel);

        }


        public async Task<IActionResult> ToggleReadStatus(int notificationId, bool IsRead)
        {
            try
            {
                await _notificationService.MarkAsReadByIdAsync(notificationId, IsRead);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while toggling read status.");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                await _notificationService.MarkAllAsReadAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while marking all notifications as read.");
                return StatusCode(500, "Internal server error");
            }
        }
        public async Task<IActionResult> MarkAllAsNotRead()
        {
            try
            {
                await _notificationService.MarkAllAsNotReadAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while marking all notifications as read.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAllNotifications()
        {
            try
            {
                await _notificationService.DeleteAllNotificationsAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting all notifications.");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> DeleteNotification(int NotificationId)
        {
            try
            {
                await _notificationService.SoftDeleteNotification(NotificationId);
                return RedirectToAction("Notifications", "Home");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetTopNotifications()
        {
            var notifications = await _notificationService.GetTopNotificationsAsync(5);

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notifications);

            return Ok();
        }
    }
}

