using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
    public interface IConfigManagement
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
    }
}