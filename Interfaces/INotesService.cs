using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Interfaces
{
  public  interface INotesService
    {
        Task<int> AddNotes(Notes Notes);
        Task<int> AddNotesMultiple(Notes Notes);
        Task<List<Notes>> GetNotes();
        Task<Notes> GetNotesByid(int Id);
        Task<Notes> UpdateNotes(Notes model);
        Task DeleteNotes(int Id);





        Task<int> AddadminNotesTagAsync(NotesTag model);
        Task<NotesTag> GetNotesTagByIdAsync(int Id);
        Task<List<NotesTag>> GetNotesTagAsync();
        Task<NotesTag> UpdateNotesTagAsync(NotesTag model);
        Task DeleteNotesTagAsync(int Id);


        Task<int> AddNotesDetails(Notes Notes);
        Task<List<Notes>> GetNotesDetails();
        Task<Notes> GetNotesDetailsByid(int Id);
        Task<Notes> UpdateNotesDetails(Notes model);
        Task DeleteNotesDetails(int Id);

    }
}
