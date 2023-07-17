using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IRequestUserService
    {
        Task<int> AddRequestUser(RequestUsers RequestUser);

        Task<List<RequestUsers>> GetRequestUser(string Status = "");
        Task<RequestUsers> GetByEmailAsync(string Email);
        Task<RequestUsers> GetRequestUserById(string Id);
        Task ChangeStatus(string Id,string Status);
    }
}

