using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class AttorneyAdmin : IAttorneyAdmin
    {
        private readonly ApiDbContext dbContext;
        public AttorneyAdmin(ApiDbContext dbContext)
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

        #region Do Not Hire
        public async Task<List<HireReason>> GetReasons()
        {
            try
            {
                List<HireReason> _resultModel = await dbContext.HireReason.ToListAsync();
                return _resultModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddReasonAsync(HireReason model)
        {
            try
            {
                await dbContext.HireReason.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.ReasonId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<HireReason> GetReasonsByIdAsync(int Id)
        {
            try
            {
                HireReason hireReason = await dbContext.HireReason.Where(x => x.ReasonId == Id).FirstOrDefaultAsync();
                return hireReason;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<HireReason> UpdateReasonAsync(HireReason model)
        {
            try
            {
                HireReason hireReason = new HireReason();
                if (model.ReasonId != 0)
                {
                    hireReason.ReasonId = model.ReasonId;
                }
                if (model.ReasonName != "")
                {
                    hireReason.ReasonName = model.ReasonName;
                }
                if (model.FirmId != 0 && model.FirmId != null)
                {
                    hireReason.FirmId = model.FirmId;
                }
                if (model.UserId != null)
                {
                    hireReason.UserId = model.UserId;
                }

                dbContext.Entry(hireReason).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return hireReason;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteReasonAsync(int Id)
        {
            try
            {
                HireReason hireReason = await dbContext.HireReason.Where(x => x.ReasonId == Id).FirstOrDefaultAsync();
                dbContext.Entry(hireReason).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Lead Status
        public async Task<List<LeadStatus>> GetLeadStatus()
        {
            try
            {
                List<LeadStatus> _resultModel = await dbContext.LeadStatus.ToListAsync();
                return _resultModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddLeadStatusAsync(LeadStatus model)
        {
            try
            {
                await dbContext.LeadStatus.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.LStatusId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<LeadStatus> GetLeadStatusByIdAsync(int Id)
        {
            try
            {
                LeadStatus leadStatus = await dbContext.LeadStatus.Where(x => x.LStatusId == Id).FirstOrDefaultAsync();
                return leadStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<LeadStatus> UpdateLeadStatusAsync(LeadStatus model)
        {
            try
            {
                LeadStatus leadStatus = new LeadStatus();
                if (model.LStatusId != 0)
                {
                    leadStatus.LStatusId = model.LStatusId;
                }
                if (model.LStatusName != "")
                {
                    leadStatus.LStatusName = model.LStatusName;
                }
                if (model.FirmId != 0 && model.FirmId != null)
                {
                    leadStatus.FirmId = model.FirmId;
                }
                if (model.UserId != null)
                {
                    leadStatus.UserId = model.UserId;
                }

                dbContext.Entry(leadStatus).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return leadStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteLeadStatusAsync(int Id)
        {
            try
            {
                LeadStatus leadStatus = await dbContext.LeadStatus.Where(x => x.LStatusId == Id).FirstOrDefaultAsync();
                dbContext.Entry(leadStatus).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Custom Field
        public async Task<List<CustomField>> GetCustomField()
        {
            try
            {
                var _resultModel = await dbContext.CustomField.ToListAsync();
                return _resultModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddCustomFieldAsync(CustomField model)
        {
            try
            {
                await dbContext.CustomField.AddAsync(model);
                await dbContext.SaveChangesAsync();

                if (model.FullPractice == true)
                {
                    var getData=dbContext.PracticeArea.ToList();
                    for (int i = 0; i < getData.Count(); i++)
                    {
                        CustomPractice customPractice = null;
                        customPractice= new CustomPractice();
                        customPractice.PracticeAreaID= getData[i].Id;
                        customPractice.FieldID = model.FieldId;
                        await dbContext.CustomPractice.AddAsync(customPractice);
                        await dbContext.SaveChangesAsync();
                    }
                }
                return model.FieldId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CustomField> GetCustomFieldByIdAsync(int Id)
        {
            try
            {
                CustomField customField = await dbContext.CustomField.Where(x => x.FieldId == Id).FirstOrDefaultAsync();
                return customField;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CustomField> UpdateCustomFieldAsync(CustomField model)
        {
            try
            {
                var getdata =dbContext.CustomField.ToList().Where(x=>x.FieldId==model.FieldId).FirstOrDefault();
                if (model.CustomFieldName != "")
                {
                    getdata.CustomFieldName = model.CustomFieldName;
                }
                dbContext.Entry(getdata).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return getdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteCustomFieldAsync(int Id)
        {
            try
            {
                CustomField customField = await dbContext.CustomField.Where(x => x.FieldId == Id).FirstOrDefaultAsync();
                dbContext.Entry(customField).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> AddCustomValueAsync(CFieldValue model)
        {
            try
            {
                await dbContext.CFieldValue.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.FieldValueID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CustomField>> GetCustomPracticeByIdAsync(int Id)
        {
            try
            {
                List<CustomField> obj = new List<CustomField>();
                var customField = await dbContext.CustomPractice.Where(x => x.PracticeAreaID == Id).ToListAsync();
                for (int i = 0; i < customField.Count(); i++)
                {
                    var listFields = dbContext.CustomField.Find(customField[i].FieldID);
                    if(listFields != null)
                    {
                        obj.Add(listFields);
                    }
                }
                var FData= obj.ToList().Where(x => x.Type == "Cases").ToList();
                return FData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region TimeEntry
        public async Task<List<TimeEntryActivity>> GetTimeEntryActivity()
        {
            try
            {
                var data = await dbContext.TimeEntryActivity.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddTimeEntryActivityAsync(TimeEntryActivity model)
        {
            try
            {
                await dbContext.TimeEntryActivity.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.ActivityId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteTimeEntryActivityAsync(int Id)
        {
            try
            {
                var data = await dbContext.TimeEntryActivity.Where(x => x.ActivityId == Id).FirstOrDefaultAsync();
                dbContext.Entry(data).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<TimeEntryActivity> UpdateTimeEntryActivityAsync(TimeEntryActivity model)
        {
            try
            {
                TimeEntryActivity data = new TimeEntryActivity();
                if (model.ActivityId != 0)
                {
                    data.ActivityId = model.ActivityId;
                }
                if (model.ActivityName != "")
                {
                    data.ActivityName = model.ActivityName;
                }
                if (model.FirmId != 0 && model.FirmId != null)
                {
                    data.FirmId = model.FirmId;
                }
                if (model.UserId != null)
                {
                    data.UserId = model.UserId;
                }

                dbContext.Entry(data).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<TimeEntryActivity> GetTimeEntryActivityByIdAsync(int Id)
        {
            try
            {
                var data = await dbContext.TimeEntryActivity.Where(x => x.ActivityId == Id).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<int> AddTimeEntryAsync(TimeEntry model)
        {
            try
            {
                await dbContext.TimeEntry.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.TimeEntryID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TimeEntry>> GetTimeEntry()
        {
            try
            {
                var data = await dbContext.TimeEntry.ToListAsync();
                var getcasename = await dbContext.CaseDetail.ToListAsync();
                var getactivity = await dbContext.TimeEntryActivity.ToListAsync();
                var getusername = await dbContext.Users.ToListAsync();
                foreach (var item in data) {
                    var name= getcasename.ToList().Where(x => x.Id == item.CaseId).FirstOrDefault();
                    if (name!=null)
                    {
                        item.Casename = name.CaseName;
                    }
                    var Activityname= getactivity.ToList().Where(x => x.ActivityId == item.ActivityId).FirstOrDefault();
                    if (Activityname!=null)
                    {
                        item.Activityname = Activityname.ActivityName;
                    }
                    var Username= getusername.ToList().Where(x => x.Id == item.UserId).FirstOrDefault();
                    if (Username != null)
                    {
                        item.Username = Username.UserName;
                    }
                    
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteTimeEntryAsync(int Id)
        {
            try
            {
                var data = await dbContext.TimeEntry.Where(x => x.TimeEntryID == Id).FirstOrDefaultAsync();
                dbContext.Entry(data).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<TimeEntry> GetTimeEntryByIdAsync(int Id)
        {
            try
            {
                var data = await dbContext.TimeEntry.Where(x => x.TimeEntryID == Id).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
