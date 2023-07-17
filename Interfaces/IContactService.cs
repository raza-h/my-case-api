using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IContactService
    {
        Task<int> AddContactAsync(Contact contact);
        Task<List<Contact>> GetContactsAsync(int contactGroupId = 0);
        Task<List<Contact>> GetContactsByCompanyIdAsync(List<int?> companyIds);
        Task<Contact> GetByIdAsync(int Id);
        Task<Contact> UpdateAsync(Contact contact);
        Task<List<Contact>> GetContactsAndCaseName();
        Task DeleteAsync(int Id);
    }
}
