using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class ContactService : IContactService
    {
        private readonly ApiDbContext dbContext;
        public ContactService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddContactAsync(Contact contact)
        {
            try
            {
                await dbContext.Contact.AddAsync(contact);
                await dbContext.SaveChangesAsync();
                return contact.ContactId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Contact>> GetContactsAsync(int contactGroupId = 0)
        {
            try
            {
                List<ContactGroup> contactGroups = await dbContext.ContactGroup.ToListAsync();
                List<Country> countries = await dbContext.Country.ToListAsync();
                List<Contact> contacts = new List<Contact>();
                if (contactGroupId > 0)
                    contacts = dbContext.Contact.Where(x => x.ContactGroupId == contactGroupId).ToList();
                else
                    contacts = await dbContext.Contact.ToListAsync();
                if(contacts !=null && contacts.Count > 0 && contactGroups != null && contacts.Count > 0 && countries != null && countries.Count > 0)
                {
                    foreach (var contact in contacts)
                    {
                        contact.ContactGroupName = contactGroups.Where(x => x.Id == contact.ContactGroupId).Select(c => c.ContactGroupName).FirstOrDefault();
                        contact.CountryName = countries.Where(x => x.Id == contact.CountryId).Select(c => c.Name).FirstOrDefault();
                    }
                } 
                return contacts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Contact> GetByIdAsync(int Id)
        {
            try
            {
                Contact contact = await dbContext.Contact.Where(x => x.ContactId == Id).FirstOrDefaultAsync();
                var customFields = await dbContext.CustomField.Where(x => x.Type == "Contacts").ToListAsync();
                if (customFields!=null)
                {
                    contact.customField= customFields;
                }
                var customFieldsValue = await dbContext.CFieldValue.Where(x => x.ConcernID == contact.ContactId).ToListAsync();
                if (customFieldsValue != null)
                {
                    contact.cfieldValue = customFieldsValue;
                }
                return contact;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        public async Task<Contact> UpdateAsync(Contact contact)
        {
            try
            {
                dbContext.Entry(contact).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return contact;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteAsync(int Id)
        {
            Contact contact = await dbContext.Contact.Where(x => x.ContactId == Id).FirstOrDefaultAsync();
            try
            {
                dbContext.Entry(contact).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Contact>> GetContactsByCompanyIdAsync(List<int?> companyIds)
        {
            try
            {
                List<Contact> contacts = await dbContext.Contact.Where(x => companyIds.Contains(x.CompanyId)).ToListAsync();
                return contacts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Contact>> GetContactsAndCaseName()
        {
            try
            {
                List<Contact> contacts = await dbContext.Contact.ToListAsync();
                List<Contact> _newcontacts = new List<Contact>();
                foreach (var item in contacts)
                {
                    Contact model = new Contact();

                    model.ContactId = item.ContactId;
                    model.FirstName = item.FirstName;
                    CaseDetail caseDetail = dbContext.CaseDetail.Where(x => x.BillingContact == item.ContactId).FirstOrDefault();
                    model.CaseName = caseDetail.CaseName;

                    _newcontacts.Add(model);
                }


                return _newcontacts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
