using Microsoft.AspNetCore.Identity;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IUserService
    {
        Task<List<string>> AddUsersBulk(List<User> users);
        Task<string> AddAsync(User model);

        Task AddUserSubscription(UserSubcription userSubcription);
        Task AddPaymentInfo(PaymentInfo payment);
        Task AddUserVerificationAsync(UserVerification userVerification);
        Task<List<User>> GetUsersAsync(string Status = "", string ParentId = "");
        Task<List<User>> GetAllUsersAsync(string ParentId = "");
        Task<List<UserDropDown>> GetUsersWithTitleAsync();
        Task<List<UserDropDown>> GetUsersAgainstClientOrStaffAsync(string userId, string type);
        Task<User> GetUserByIdAsync(string Id);
        Task<List<User>> GetBlockedUsersAsync(string ParentId);
        Task ToggleUserBlock(string UserId, bool Status);
        Task DeleteUserAsync(string Email);
        Task<List<IdentityRole>> GetRolesAsync();
        Task<User> GetByEmailAsync(string Email);
        Task DeleteRoleAsync(IdentityRole Role);
        Task<User> UpdateUserAsync(User user);
        Task AddRoleAsync(string roleName);
        Task ChangeStatus(string Id, string Status);
        Task AddTestAsync(TestUser model);
    }
}
