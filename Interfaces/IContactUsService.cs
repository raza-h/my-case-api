using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IContactUsService
    {
        Task<int> AddContactAsync(ContactUs model);
        Task<List<ContactUs>> GetContactAsync();
        Task<ContactUs> GetContactByIdAsync(int Id);
        Task<ContactUs> UpdateContactAsync(ContactUs model);
        Task DeleteContactAsync(int Id);

    }
}
