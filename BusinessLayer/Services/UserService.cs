using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole<int>> roleManager;
        private readonly IUnitOfWork unitOfWork;

        public UserService(SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole<int>> roleManager, IUnitOfWork unitOfWork)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IdentityResult> AddUserToRoleAsync(UserDto userDto, string roleName)
        {
            unitOfWork.BeginTransaction();
            try
            {
                var user = mapper.Map<User>(userDto);
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    await unitOfWork.CompleteAsync();
                    unitOfWork.Commit();
                }
                return result;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> CheckPasswordAsync(UserDto userDto, string CurrentPassword)
        {
            var currentUser = await userManager.FindByNameAsync(userDto.UserName);
            var user = mapper.Map<User>(currentUser);
            var passwordCheck = await userManager.CheckPasswordAsync(user, CurrentPassword);
            return passwordCheck;
        }

        public async Task<IdentityResult> DeleteUserAsync(UserDto userDto)
        {
            unitOfWork.BeginTransaction();
            try
            {
                var user = await userManager.FindByIdAsync(userDto.Id.ToString());
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    await unitOfWork.CompleteAsync();
                    unitOfWork.Commit();
                }
                return result;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> FindRole(string roleName)
        {
            return await roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IEnumerable<string>> GetAllRoles()
        {
            return await roleManager.Roles.Select(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await userManager.Users.ToListAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }



        public async Task<UserDto> GetUserByNameAsync(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            return mapper.Map<UserDto>(user);
        }

        public async Task<IList<string>> GetUserRolesAsync(UserDto userDto)
        {
            var user = mapper.Map<User>(userDto);
            return await userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable<UserDto>> GetUsersInRoleAsync(string roleName)
        {
            var users = await userManager.GetUsersInRoleAsync(roleName);
            return mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<bool> IsFindByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> IsInRoleAsync(UserDto userDto, string roleName)
        {
            var user = mapper.Map<User>(userDto);
            return await userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(UserDto userDto, string roleName)
        {
            var user = mapper.Map<User>(userDto);
            var result = await userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                await userManager.UpdateAsync(user);
            }
            return result;

        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordDto passwordDto, string currentUserName)
        {
            var user = await userManager.FindByNameAsync(currentUserName);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            if (passwordDto.NewPassword != passwordDto.ConfirmPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "New password and confirmation do not match." });
            }
            var result = await userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);
            if (result.Succeeded)
            {
                await signInManager.RefreshSignInAsync(user);
            }

            return result;
        }


        public async Task<IdentityResult> SetUserNameAsync(UserDto userDto, string newUserName)
        {
            var currentUser = await userManager.FindByNameAsync(userDto.UserName);
            if (currentUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }
            var result = await userManager.SetUserNameAsync(currentUser, newUserName);
            if (!result.Succeeded)
            {
                return result;
            }
            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(currentUser, isPersistent: false);

            return IdentityResult.Success;
        }


        public async Task<SignInResult> SignInAsync(UserDto userDto, string password, bool rememberMe = false)
        {
            var user = await userManager.FindByNameAsync(userDto.UserName);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            return await signInManager.CheckPasswordSignInAsync(user, password, rememberMe);
        }

        public async Task SignInAsync(UserDto userDto, bool isPersistent)
        {
            var user = mapper.Map<User>(userDto);
            await signInManager.SignInAsync(user, isPersistent);
        }

        public async Task SignInUserAsync(UserDto userDto,bool isPersistent )
        {
            var user = await userManager.FindByNameAsync(userDto.UserName);
            if (user != null)
            {
                await signInManager.SignInAsync(user,isPersistent);

            }
        }

        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateSecurityStampAsync(UserDto userDto)
        {
            var currentUser = await userManager.FindByNameAsync(userDto.UserName);

            var user = mapper.Map<User>(currentUser);

            return await userManager.UpdateSecurityStampAsync(user);
        }
        public async Task<IdentityResult> UpdateUserAsync(UserDto userDto)
        {
            unitOfWork.BeginTransaction();
            try
            {
                var currentUser = await userManager.FindByNameAsync(userDto.UserName);

                if (currentUser == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }
                userDto.Id = currentUser.Id;
                // Map the changes from userDto to the currentUser
                mapper.Map(userDto, currentUser);
                var User = mapper.Map<User>(currentUser);
                var result = await userManager.UpdateAsync(User);
                if (result.Succeeded)
                {
                    await unitOfWork.CompleteAsync();
                    unitOfWork.Commit();
                }
                else
                {
                    unitOfWork.Rollback();
                }

                return result;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
        public async Task<UserDto> CheckEmailIsFindAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var UserDto = mapper.Map<UserDto>(user);
            return UserDto;
        }
        public async Task<UserDto> CheckUserNameIsFindAsync(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            var UserDto = mapper.Map<UserDto>(user);
            return UserDto;
        }
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            var UserDto = mapper.Map<UserDto>(user);
            return UserDto;
        }
    }
}
