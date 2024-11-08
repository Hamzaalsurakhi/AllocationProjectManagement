using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PresentationLayer.ViewModel;
using PresentationLayer.ViewModels;

namespace PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;


        public AccountController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await userService.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // Redirect authenticated users to their respective pages
            if (User.Identity.IsAuthenticated)
            {
                var currentUserName = User.Identity.Name;
                var user = await userService.GetUserByNameAsync(currentUserName);

                if (user != null)
                {
                    if (await userService.IsInRoleAsync(user, "Project Delivery Manager"))
                    {
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else if (await userService.IsInRoleAsync(user, "HR Officer"))
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                }
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userService.GetUserByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.UserName), "UserName Not Found");
                    return View(model);
                    }
         

                    var result = await userService.SignInAsync(user, model.Password, model.RememberMe);
                    if (result.Succeeded)
                        {
                        await userService.SignInUserAsync(user,model.RememberMe);
                        if (await userService.IsInRoleAsync(user, "Project Delivery Manager"))
                            {

                            return RedirectToAction("Dashboard", "Home");
                            }
                        else
                        if (await userService.IsInRoleAsync(user, "HR Officer"))
                            {
                            return RedirectToAction("Index", "Employee");
                            }
                        else
                            {
                            return RedirectToAction("Index", "Home");
                            }
                        }
                    else
                        {
                        ModelState.AddModelError(nameof(model.Password), "Password is incorrect");
                        }

                    if (ModelState.ErrorCount > 0)
                        return View(model);
            }
            return View(model);
        }
        [Authorize]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUserName = User.Identity.Name;
            var UserDto = await userService.GetUserByNameAsync(currentUserName);
            var checkPassword = await userService.CheckPasswordAsync(UserDto, model.CurrentPassword);
            if (checkPassword)
            {
                var passwordDto = mapper.Map<ResetPasswordDto>(model);

                var result = await userService.ResetPassword(passwordDto, currentUserName);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Password has been reset successfully.";
                }
            }
            else
            {
                ModelState.AddModelError(nameof(model.CurrentPassword), "Current Password is not Match");
            }


            return View();
        }


    }
}
