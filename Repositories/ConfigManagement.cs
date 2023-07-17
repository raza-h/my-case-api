using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class ConfigManagement : IConfigManagement
    {
        private readonly ApiDbContext dbContext;
        public ConfigManagement(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #region Contact Group
        public async Task<int> AddContactGroupAsync(ContactGroup contactGroup)
        {
            try
            {
                await dbContext.ContactGroup.AddAsync(contactGroup);
                await dbContext.SaveChangesAsync();
                return contactGroup.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ContactGroup> GetContactGroupByIdAsync(int Id)
        {
            try
            {
                ContactGroup contactGroup = await dbContext.ContactGroup.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return contactGroup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ContactGroup>> GetContactGroupsAsync()
        {
            try
            {
                List<ContactGroup> contactGroups = await dbContext.ContactGroup.ToListAsync();
                return contactGroups;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ContactGroup> UpdateContactGroupAsync(ContactGroup contactGroup)
        {
            try
            {
                dbContext.Entry(contactGroup).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return contactGroup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteContactGroupAsync(int Id)
        {
            try
            {
                ContactGroup contactGroup = await dbContext.ContactGroup.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (contactGroup != null)
                {
                    dbContext.Entry(contactGroup).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Practice Area
        public async Task<int> AddPracticeAreaAsync(PracticeArea practiceArea)
        {
            try
            {
                await dbContext.PracticeArea.AddAsync(practiceArea);
                await dbContext.SaveChangesAsync();
                return practiceArea.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PracticeArea> GetPracticeAreaByIdAsync(int Id)
        {
            try
            {
                PracticeArea practiceArea = await dbContext.PracticeArea.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return practiceArea;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<PracticeArea>> GetPracticeAreasAsync()
        {
            try
            {
                List<PracticeArea> practiceAreas = await dbContext.PracticeArea.ToListAsync();
                if(practiceAreas != null && practiceAreas.Count > 0)
                {
                    List<CaseDetail> caseDetails = await dbContext.CaseDetail.ToListAsync();
                    foreach(var practiceArea in practiceAreas)
                    {
                        practiceArea.ActiveCases = caseDetails == null ? 0 : caseDetails.Where(x => x.PracticeArea == practiceArea.Id) != null ? caseDetails.Where(x => x.PracticeArea == practiceArea.Id).Count() : 0;
                    }
                }
                return practiceAreas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PracticeArea> UpdatePracticeAreaAsync(PracticeArea practiceArea)
        {
            try
            {
                dbContext.Entry(practiceArea).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return practiceArea;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeletePracticeAreaAsync(int Id)
        {
            try
            {
                PracticeArea practiceArea = await dbContext.PracticeArea.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (practiceArea != null)
                {
                    dbContext.Entry(practiceArea).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region User Title
        public async Task<int> AddUserTitleAsync(UserTitle userTitle)
        {
            try
            {
                await dbContext.UserTitle.AddAsync(userTitle);
                await dbContext.SaveChangesAsync();
                return userTitle.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserTitle> GetUserTitleByIdAsync(int Id)
        {
            try
            {
                UserTitle userTitle = await dbContext.UserTitle.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return userTitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserTitle>> GetUserTitlesAsync()
        {
            try
            {
                List<UserTitle> userTitles = await dbContext.UserTitle.ToListAsync();
                return userTitles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserTitle> UpdateUserTitleAsync(UserTitle userTitle)
        {
            try
            {
                dbContext.Entry(userTitle).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return userTitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteUserTitleAsync(int Id)
        {
            try
            {
                UserTitle userTitle = await dbContext.UserTitle.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (userTitle != null)
                {
                    dbContext.Entry(userTitle).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Refferal Source
        public async Task<int> AddRefferalSourceAsync(RefferalSource refferalSource)
        {
            try
            {
                await dbContext.RefferalSource.AddAsync(refferalSource);
                await dbContext.SaveChangesAsync();
                return refferalSource.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RefferalSource> GetRefferalSourceByIdAsync(int Id)
        {
            try
            {
                RefferalSource refferalSource = await dbContext.RefferalSource.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return refferalSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RefferalSource>> GetRefferalSourcesAsync()
        {
            try
            {
                List<RefferalSource> refferalSources = await dbContext.RefferalSource.ToListAsync();
                return refferalSources;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RefferalSource> UpdateRefferalSourceAsync(RefferalSource refferalSource)
        {
            try
            {
                dbContext.Entry(refferalSource).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return refferalSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteRefferalSourceAsync(int Id)
        {
            try
            {
                RefferalSource refferalSource = await dbContext.RefferalSource.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (refferalSource != null)
                {
                    dbContext.Entry(refferalSource).State = EntityState.Deleted;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #region Billing Method
        public async Task<int> AddBillingMethodAsync(BillingMethod billingMethod)
        {
            try
            {
                await dbContext.BillingMethod.AddAsync(billingMethod);
                await dbContext.SaveChangesAsync();
                return billingMethod.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BillingMethod> GetBillingMethodByIdAsync(int Id)
        {
            try
            {
                BillingMethod billingMethod = await dbContext.BillingMethod.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return billingMethod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<BillingMethod>> GetBillingMethodsAsync()
        {
            try
            {
                List<BillingMethod> billingMethods = await dbContext.BillingMethod.ToListAsync();
                return billingMethods;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BillingMethod> UpdateBillingMethodAsync(BillingMethod billingMethod)
        {
            try
            {
                dbContext.Entry(billingMethod).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return billingMethod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteBillingMethodAsync(int Id)
        {
            try
            {
                BillingMethod billingMethod = await dbContext.BillingMethod.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(billingMethod).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion#region Billing Method

        #region Note Tag
        public async Task<int> AddNotesTagAsync(NotesTag model)
        {
            try
            {
                await dbContext.NotesTag.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<NotesTag> GetNotesTagByIdAsync(int Id)
        {
            try
            {
                NotesTag _noteTag = await dbContext.NotesTag.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return _noteTag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<NotesTag>> GetNotesTagAsync()
        {
            try
            {
                List<NotesTag> _resultModel = await dbContext.NotesTag.ToListAsync();
                return _resultModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<NotesTag> UpdateNotesTagAsync(NotesTag model)
        {
            try
            {
                NotesTag notesTag = new NotesTag();
                if (model.Id != 0)
                {
                    notesTag.Id=model.Id;
                }
                if (model.NotesTagName != "")
                {
                    notesTag.NotesTagName=model.NotesTagName;
                }
                if (model.FirmId != 0 && model.FirmId!=null)
                {
                    notesTag.FirmId=model.FirmId;
                }

                dbContext.Entry(notesTag).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return notesTag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteNotesTagAsync(int Id)
        {
            try
            {
                NotesTag _notesTag = await dbContext.NotesTag.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(_notesTag).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Document Tag
        public async Task<int> AddDocumentTagAsync(DocumentTag model)
        {
            try
            {
                await dbContext.DocumentTag.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DocumentTag> GetDocumentTagByIdAsync(int Id)
        {
            try
            {
                DocumentTag _documentTag = await dbContext.DocumentTag.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return _documentTag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DocumentTag>> GetDocumentTagAsync()
        {
            try
            {
                List<DocumentTag> _resultModel = await dbContext.DocumentTag.ToListAsync();
                return _resultModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DocumentTag> UpdateDocumentTagAsync(DocumentTag model)
        {
            try
            {
                dbContext.Entry(model).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteDocumentTagAsync(int Id)
        {
            try
            {
                DocumentTag _documentTag = await dbContext.DocumentTag.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(_documentTag).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


    }
}
