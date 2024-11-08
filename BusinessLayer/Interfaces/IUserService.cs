using BusinessLayer.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> AddUserToRoleAsync(UserDto userDto, string roleName);
        Task<IdentityResult> DeleteUserAsync(UserDto userDto);
        Task<bool> FindRole(string roleName);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByNameAsync(string userName);
        Task<IList<string>> GetUserRolesAsync(UserDto userDto);
        Task<IEnumerable<UserDto>> GetUsersInRoleAsync(string roleName);
        Task<bool> IsFindByEmail(string email);
        Task<bool> IsInRoleAsync(UserDto userDto, string roleName);
        Task<IdentityResult> RemoveUserFromRoleAsync(UserDto userDto, string roleName);
        Task<SignInResult> SignInAsync(UserDto userDto, string password, bool RememberMe = false);
        Task SignInUserAsync(UserDto userDto, bool isPersistent );
        Task SignOutAsync();
        Task<IdentityResult> UpdateUserAsync(UserDto userDto);
        Task<IEnumerable<string>> GetAllRoles();
        Task<IdentityResult> SetUserNameAsync(UserDto userDto, string UserName);
        Task<IdentityResult> UpdateSecurityStampAsync(UserDto userDto);
        Task SignInAsync(UserDto userDto, bool isPersistent);
        Task<IdentityResult> ResetPassword(ResetPasswordDto passwordDto, string currentUserName);
        Task<bool> CheckPasswordAsync(UserDto userDto, string CurrentPassword);

        Task<UserDto> CheckUserNameIsFindAsync(string userName);

        Task<UserDto> CheckEmailIsFindAsync(string email);
    }
}
