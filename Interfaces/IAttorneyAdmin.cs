using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IAttorneyAdmin
    {
        #region Contact Group
        Task<int> AddContactGroupAsync(ContactGroup contactGroup);
        Task<List<ContactGroup>> GetContactGroupsAsync();
        Task<ContactGroup> GetContactGroupByIdAsync(int Id);
        Task<ContactGroup> UpdateContactGroupAsync(ContactGroup contactGroup);
        Task DeleteContactGroupAsync(int Id);
        #endregion

        #region Practice Area
        Task<int> AddPracticeAreaAsync(PracticeArea practiceArea);
        Task<List<PracticeArea>> GetPracticeAreasAsync();
        Task<PracticeArea> GetPracticeAreaByIdAsync(int Id);
        Task<PracticeArea> UpdatePracticeAreaAsync(PracticeArea practiceArea);
        Task DeletePracticeAreaAsync(int Id);
        #endregion

        #region User Title
        Task<int> AddUserTitleAsync(UserTitle userTitle);
        Task<List<UserTitle>> GetUserTitlesAsync();
        Task<UserTitle> GetUserTitleByIdAsync(int Id);
        Task<UserTitle> UpdateUserTitleAsync(UserTitle userTitle);
        Task DeleteUserTitleAsync(int Id);
        #endregion

        #region Refferal Source
        Task<int> AddRefferalSourceAsync(RefferalSource refferalSource);
        Task<List<RefferalSource>> GetRefferalSourcesAsync();
        Task<RefferalSource> GetRefferalSourceByIdAsync(int Id);
        Task<RefferalSource> UpdateRefferalSourceAsync(RefferalSource refferalSource);
        Task DeleteRefferalSourceAsync(int Id);
        #endregion

        #region Billing Method
        Task<int> AddBillingMethodAsync(BillingMethod billingMethod);
        Task<List<BillingMethod>> GetBillingMethodsAsync();
        Task<BillingMethod> GetBillingMethodByIdAsync(int Id);
        Task<BillingMethod> UpdateBillingMethodAsync(BillingMethod billingMethod);
        Task DeleteBillingMethodAsync(int Id);
        #endregion

        #region Note Tag
        Task<int> AddNotesTagAsync(NotesTag model);
        Task<List<NotesTag>> GetNotesTagAsync();
        Task<NotesTag> GetNotesTagByIdAsync(int Id);
        Task<NotesTag> UpdateNotesTagAsync(NotesTag model);
        Task DeleteNotesTagAsync(int Id);
        #endregion

        #region Document Tag
        Task<int> AddDocumentTagAsync(DocumentTag model);
        Task<List<DocumentTag>> GetDocumentTagAsync();
        Task<DocumentTag> GetDocumentTagByIdAsync(int Id);
        Task<DocumentTag> UpdateDocumentTagAsync(DocumentTag model);
        Task DeleteDocumentTagAsync(int Id);
        #endregion

        #region Do Not Hire
        Task<List<HireReason>> GetReasons();
        Task<int> AddReasonAsync(HireReason model);
        Task<HireReason> GetReasonsByIdAsync(int Id);
        Task<HireReason> UpdateReasonAsync(HireReason model);
        Task DeleteReasonAsync(int Id);
        #endregion

        #region Lead Status
        Task<List<LeadStatus>> GetLeadStatus();
        Task<int> AddLeadStatusAsync(LeadStatus model);
        Task<LeadStatus> GetLeadStatusByIdAsync(int Id);
        Task<LeadStatus> UpdateLeadStatusAsync(LeadStatus model);
        Task DeleteLeadStatusAsync(int Id);
        #endregion

        #region Custom Field
        Task<List<CustomField>> GetCustomField();
        Task<int> AddCustomFieldAsync(CustomField model);
        Task<CustomField> GetCustomFieldByIdAsync(int Id);
        Task<CustomField> UpdateCustomFieldAsync(CustomField model);
        Task DeleteCustomFieldAsync(int Id);
        Task<int> AddCustomValueAsync(CFieldValue model);
        Task<List<CustomField>> GetCustomPracticeByIdAsync(int Id);
        #endregion

        #region TimeEntry
        Task<List<TimeEntryActivity>> GetTimeEntryActivity();
        Task<int> AddTimeEntryActivityAsync(TimeEntryActivity model);
        Task DeleteTimeEntryActivityAsync(int Id);
        Task<TimeEntryActivity> UpdateTimeEntryActivityAsync(TimeEntryActivity model);
        Task<TimeEntryActivity> GetTimeEntryActivityByIdAsync(int Id);


        Task<int> AddTimeEntryAsync(TimeEntry model);
        Task<List<TimeEntry>> GetTimeEntry();
        Task DeleteTimeEntryAsync(int Id);
        Task<TimeEntry> GetTimeEntryByIdAsync(int Id);
        #endregion

    }
}