using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApiDbContext dbContext;
        private readonly IMapper mapper;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApiDbContext dbContext, IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<string> AddAsync(User model)
        {
            try
            {
                model.UserName = model.Email;
                model.Status = true;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    model.PasswordHash = userManager.PasswordHasher.HashPassword(model, model.Password);
                    await userManager.CreateAsync(model, model.Password);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    await userManager.CreateAsync(model);

                }
                
                bool adminRoleExists = await roleManager.RoleExistsAsync(model.RoleName);
                if (!adminRoleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(model.RoleName));
                    await dbContext.SaveChangesAsync();

                }
                await userManager.AddToRoleAsync(model, model.RoleName);
                await dbContext.SaveChangesAsync();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<string>> AddUsersBulk(List<User> users)
        {
            try
            {
                List<string> Emails = new List<string>();
                await dbContext.User.AddRangeAsync(users);
                if (users != null && users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        User existingUser = await GetByEmailAsync(user.Email);
                        if (existingUser == null)
                        {
                            user.UserName = user.Email;
                            user.Status = true;
                            user.PasswordHash = "123";
                            user.Password = "123";

                            if (!string.IsNullOrEmpty(user.Password))
                            {
                                user.PasswordHash = userManager.PasswordHasher.HashPassword(user, user.Password);
                                await userManager.CreateAsync(user, user.Password);
                            }
                            else
                                await userManager.CreateAsync(user);

                            await userManager.AddToRoleAsync(user, user.RoleName);
                            await dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            Emails.Add(user.Email);
                        }
                    }
                }
                return Emails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task AddUserSubscription(UserSubcription userSubcription)
        {
            try
            {
                await dbContext.UserSubcription.AddAsync(userSubcription);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task AddPaymentInfo(PaymentInfo payment)
        {
            try
            {
                string newAcode = string.Empty;
                var list = await dbContext.Payment.ToListAsync();
                if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(list.LastOrDefault().InvoiceNo))
                {
                    var splitedCode = list.LastOrDefault().InvoiceNo.Split('-');
                    int lastPartOfCode = Convert.ToInt32(splitedCode[2]) + 1;
                    newAcode = $"SI-{DateTime.Now.Year}-000" + Convert.ToString(lastPartOfCode);
                }
                else
                {
                    newAcode = $"SI-{DateTime.Now.Year}-0001";
                }
                payment.InvoiceNo = newAcode;
                await dbContext.Payment.AddAsync(payment);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task AddUserVerificationAsync(UserVerification userVerification)
        {
            try
            {
                await dbContext.UserVerification.AddAsync(userVerification);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteUserAsync(string Id)
        {
            try
            {
                User user = await userManager.FindByIdAsync(Id);
                await userManager.DeleteAsync(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> GetByEmailAsync(string Email)
        {
            try
            {
                User user = await dbContext.User.Where(x => x.Email == Email).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<User>> GetUsersAsync(string Status = "", string ParentId = "")
        {
            try
            {
                List<User> users = new List<User>();
                if (!string.IsNullOrEmpty(ParentId))
                {
                    var currentUser = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                    users = await dbContext.User.Where(x => x.ParentId == currentUser.Id).ToListAsync();
                }
                if (users != null && users.Count > 0 && Status == "leadAttorney")
                    users = users.Where(x => x.RoleName == "Attorney").ToList();
                else if (Status == "Approved")
                    users = await dbContext.User.Where(x => x.VerificationStatus == VerificationStatus.Approved).ToListAsync();
                else if (Status == "Pending")
                    users = await dbContext.User.Where(x => x.VerificationStatus == VerificationStatus.Pending).ToListAsync();
                else if (Status == "Rejected")
                    users = await dbContext.User.Where(x => x.VerificationStatus == VerificationStatus.Rejected).ToListAsync();

                if (users != null && users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        user.ImagePath = !string.IsNullOrEmpty(user.ImagePath) ? user.ImagePath : "Images/blank-profile.png";
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<User>> GetAllUsersAsync(string ParentId = "")
        {
            try
            {
                List<User> users = new List<User>();
                if (!string.IsNullOrEmpty(ParentId))
                {
                    var currentUser = dbContext.User.ToList().Where(x => x.Id == ParentId).FirstOrDefault();
                    users = await dbContext.User.Where(x => x.ParentId == currentUser.Id && x.VerificationStatus!= VerificationStatus.Rejected).ToListAsync();
                }
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<UserDropDown>> GetUsersWithTitleAsync()
        {
            try
            {
                var users = await dbContext.User.ToListAsync();
                var userTitles = await dbContext.UserTitle.ToListAsync();
                List<UserDropDown> userDropDown = mapper.Map<List<UserDropDown>>(users);
                if (userDropDown != null && userDropDown.Count > 0)
                {
                    foreach (var user in userDropDown)
                    {
                        user.UserTitle = userTitles.Where(x => x.Id == user.UserTitleId).Select(u => u.UserTitleName).FirstOrDefault() != null ? userTitles.Where(x => x.Id == user.UserTitleId).Select(u => u.UserTitleName).FirstOrDefault() : "";
                        user.LastName = user.LastName != null ? user.LastName : "";
                    }
                }
                return userDropDown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserDropDown>> GetUsersAgainstClientOrStaffAsync(string userId, string type)
        {
            try
            {
                var users = await dbContext.User.ToListAsync();
                List<UserDropDown> userDropDown = new List<UserDropDown>();
                var caseDetails = await dbContext.CaseDetail.Where(c => c.BillingContact != null && c.BillingContact > 0).ToListAsync();
                var userAgainstCases = await dbContext.UserAgainstCase.ToListAsync();
                if (type == "Client")
                {
                    var contact = await dbContext.Contact.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                    if (contact != null)
                    {
                        var relatedCasesIds = (caseDetails.Where(x => x.BillingContact == contact.ContactId).ToList() != null && caseDetails.Where(x => x.BillingContact == contact.ContactId).ToList().Count > 0) ?
                                       caseDetails.Where(x => x.BillingContact == contact.ContactId).ToList().Select(c => c.Id).ToList() : new List<int>();
                        var userIds = userAgainstCases.Where(x => x.CaseId != null && relatedCasesIds.Contains(x.CaseId.Value)).ToList().Select(u => u.UserId).ToList();
                        string parentId = users.Where(x => x.Id == userId).FirstOrDefault() != null ? users.Where(x => x.Id == userId).FirstOrDefault().ParentId : "";
                        if (!string.IsNullOrEmpty(parentId))
                            userIds.Add(parentId);

                        users = users.Where(x => userIds.Contains(x.Id) && x.Id != userId).ToList();
                        if (users != null && users.Count > 0)
                        {
                            foreach (var user in users)
                            {
                                user.ImagePath = !string.IsNullOrEmpty(user.ImagePath) ? user.ImagePath : "Images/blank-profile.png";
                            }
                        }
                        userDropDown = mapper.Map<List<UserDropDown>>(users);
                    }
                }
                else
                {
                    var contacts = await dbContext.Contact.ToListAsync();
                    var caseIds = userAgainstCases.Where(x => x.UserId == userId && x.CaseId != null).ToList() != null ?
                        userAgainstCases.Where(x => x.UserId == userId && x.CaseId != null).ToList().Select(u => u.CaseId).ToList() : new List<int?>();
                    var contactIds = caseDetails.Where(x => caseIds.Contains(x.Id)).ToList() != null ?
                        caseDetails.Where(x => caseIds.Contains(x.Id)).ToList().Select(c => c.BillingContact).ToList() : new List<int?>();
                    var userIds = contacts.Where(x => contactIds.Contains(x.ContactId)).ToList() != null ?
                        contacts.Where(x => contactIds.Contains(x.ContactId)).ToList().Select(x => x.UserId).ToList() : new List<string>();

                    string parentId = users.Where(x => x.Id == userId).FirstOrDefault() != null ? users.Where(x => x.Id == userId).FirstOrDefault().ParentId : "";
                    if (!string.IsNullOrEmpty(parentId))
                        userIds.Add(parentId);

                    users = users.Where(x => userIds.Contains(x.Id) && x.Id != userId).ToList();
                    if (users != null && users.Count > 0)
                    {
                        foreach (var user in users)
                        {
                            user.ImagePath = !string.IsNullOrEmpty(user.ImagePath) ? user.ImagePath : "Images/blank-profile.png";
                        }
                    }
                    userDropDown = mapper.Map<List<UserDropDown>>(users);
                }
                return userDropDown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> GetUserByIdAsync(string Id)
        {
            try
            {
                User userModel = await userManager.FindByIdAsync(Id);
                return userModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> UpdateUserAsync(User model)
        {
            try
            {
                var user = dbContext.User.First(x => x.Id == model.Id);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PostCode = model.PostCode;
                user.PhoneNumber = model.PhoneNumber;
                user.City = model.City;
                user.ImagePath = model.ImagePath;
                user.ParentId = model.Id;
                dbContext.Entry(user).CurrentValues.SetValues(user);
                await dbContext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<User>> GetBlockedUsersAsync(string ParentId)
        {
            try
            {
                var Blockedusers = await dbContext.User.Where(x => x.ParentId == ParentId && x.Status == false).ToListAsync();
                return Blockedusers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ToggleUserBlock(string UserId, bool Status)
        {
            try
            {
                var user = dbContext.User.First(x => x.Id == UserId);
                user.Status = Status;
                dbContext.Entry(user).CurrentValues.SetValues(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task AddRoleAsync(string roleName)
        {
            try
            {
                IdentityRole role = new IdentityRole
                {
                    Name = roleName
                };
                await roleManager.CreateAsync(role);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            try
            {
                var roles = await roleManager.Roles.ToListAsync();
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteRoleAsync(IdentityRole Role)
        {
            try
            {
                await roleManager.DeleteAsync(Role);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task ChangeStatus(string Id, string Status)
        {
            try
            {
                User userModel = await userManager.FindByIdAsync(Id);

                if (Status == "Approved")
                {
                    userModel.VerificationStatus = VerificationStatus.Approved;
                    dbContext.Entry(userModel).CurrentValues.SetValues(userModel);
                    await dbContext.SaveChangesAsync();
                }
                else if (Status == "Pending")
                {
                    userModel.VerificationStatus = VerificationStatus.Pending;
                    dbContext.Entry(userModel).CurrentValues.SetValues(userModel);
                    await dbContext.SaveChangesAsync();
                }
                else if (Status == "Rejected")
                {
                    userModel.VerificationStatus = VerificationStatus.Rejected;
                    dbContext.Entry(userModel).CurrentValues.SetValues(userModel);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddTestAsync(TestUser model)
        {
            try
            {
                await dbContext.TestUser.AddAsync(model);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
