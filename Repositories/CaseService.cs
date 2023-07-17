using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class CaseService : ICaseService
    {
        private readonly ApiDbContext dbContext;
        private readonly IContactService contactService;

        public CaseService(ApiDbContext dbContext, IContactService contactService)
        {
            this.dbContext = dbContext;
            this.contactService = contactService;
        }
        public async Task<int> AddCase(CaseDetail caseDetail)
        {
            try
            {
                await dbContext.CaseDetail.AddAsync(caseDetail);
                await dbContext.SaveChangesAsync();
                return caseDetail.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CaseDetail>> GetCases()
        {
            try
            {
                List<CaseDetail> _resultModel = new List<CaseDetail>();

                List<CaseDetail> cases = await dbContext.CaseDetail.ToListAsync();
                List<Contact> contacts = await dbContext.Contact.ToListAsync();

                foreach (var item in cases)
                {
                    CaseDetail model = new CaseDetail();
                    model.BillingContact = item.BillingContact;
                    model.BillingMethod = item.BillingMethod;
                    model.CaseName = item.CaseName;
                    model.CaseNumber = item.CaseNumber;
                    model.CaseRate = item.CaseRate;
                    model.CaseStage = item.CaseStage;
                    model.ConflictCheckNotes = item.ConflictCheckNotes;
                    model.DateAppend = item.DateAppend;
                    model.Description = item.Description;
                    model.Id = item.Id;
                    model.JobTitle = item.JobTitle;
                    model.LeadAttorney = item.LeadAttorney;
                    model.Office = item.Office;
                    model.OriginatingLeadAttorney = item.OriginatingLeadAttorney;
                    model.PracticeArea = item.PracticeArea;
                    model.StatueOfLimitation = item.StatueOfLimitation;
                    model.CaseAddedBy = item.CaseAddedBy;
                    model.FirmId = item.FirmId;

                    var Contact = contacts.Where(x => x.ContactId == item.BillingContact).FirstOrDefault();
                    if (Contact != null)
                        model.ClientName = Contact.FirstName + " " + Contact.LastName;
                    else
                        model.ClientName = "N/A";
                    _resultModel.Add(model);
                }
                return _resultModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CaseDetail> GetCasesById(int id)
        {
            try
            {
                CaseDetail cases = await dbContext.CaseDetail.Where(x => x.Id == id).FirstOrDefaultAsync();
                return cases;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CaseDetail>> GetCasesByClientId(string userId)
        {
            try
            {
                List<CaseDetail> cases = new List<CaseDetail>();
                Contact contact = await dbContext.Contact.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (contact != null)
                {
                    List<UserAgainstCase> userAgainstCases = await dbContext.UserAgainstCase.ToListAsync();
                    List<User> users = await dbContext.User.ToListAsync();
                    cases = await dbContext.CaseDetail.Where(x => x.BillingContact == contact.ContactId).ToListAsync();
                    string leadId = string.Empty;
                    if (cases != null && cases.Count > 0)
                    {
                        foreach (var clientCase in cases)
                        {
                            leadId = userAgainstCases.Where(x => x.CaseId == clientCase.Id).FirstOrDefault() != null ? userAgainstCases.Where(x => x.CaseId == clientCase.Id).FirstOrDefault().UserId : "";
                            if (users.Where(x => x.Id == leadId).FirstOrDefault() != null)
                                clientCase.LeadAttorney = !string.IsNullOrEmpty(leadId) ? $"{users.Where(x => x.Id == leadId).FirstOrDefault().FirstName} {users.Where(x => x.Id == leadId).FirstOrDefault().LastName}" : "";
                        }
                    }
                }
                return cases;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CaseDetail>> GetCasesByStaffId(string userId)
        {
            try
            {
                List<CaseDetail> cases = new List<CaseDetail>();
                List<UserAgainstCase> userAgainstCases = await dbContext.UserAgainstCase.Where(x => x.UserId == userId).ToListAsync();
                if (userAgainstCases != null && userAgainstCases.Count > 0)
                {
                    List<int?> CaseIds = userAgainstCases.Select(x => x.CaseId).ToList();
                    List<User> users = await dbContext.User.ToListAsync();
                    cases = await dbContext.CaseDetail.Where(x => CaseIds.Contains(x.Id)).ToListAsync();
                    string leadId = string.Empty;
                    if (cases != null && cases.Count > 0)
                    {
                        foreach (var clientCase in cases)
                        {
                            leadId = clientCase.CaseAddedBy;
                            clientCase.LeadAttorney = $"{users.Where(x => x.Id == leadId).FirstOrDefault().FirstName} {users.Where(x => x.Id == leadId).FirstOrDefault().LastName}";
                        }
                    }
                }
                return cases;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientLocation> GetCasesLocation(string userId, int CaseId)
        {
            try
            {
                List<ClientLocation> clientLocations = new List<ClientLocation>();
                clientLocations = await dbContext.ClientLocation.ToListAsync();
                var location = clientLocations.Where(x => x.CaseId == CaseId && x.UserId == userId).FirstOrDefault();
                return location;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddCasesLocation(ClientLocation model)
        {
            try
            {
                await dbContext.ClientLocation.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.ClientLocationId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> CloseCase(int id)
        {
            try
            {
                var getCase = dbContext.CaseDetail.ToList().Where(x => x.Id == id).FirstOrDefault();
                if (getCase != null)
                {
                    getCase.IsOpen = false;
                    getCase.DateClosed = DateTime.Now;
                    dbContext.Entry(getCase).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return getCase.Id;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
