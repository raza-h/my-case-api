using Microsoft.EntityFrameworkCore;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class NotesServices : INotesService
    {
        private readonly ApiDbContext dbContext;
        public NotesServices(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddNotes(Notes model)
        {
            try
            {
                model.CreatedDate = DateTime.Now.ToString("dd-MM-yyyy");
                await dbContext.Notes.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> AddNotesMultiple(Notes model)
        {
            try
            {
                model.CreatedDate = DateTime.Now.ToString("dd-MM-yyyy");
                await dbContext.Notes.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Notes>> GetNotes()
        {
            try
            {
                List<Notes> Notes = await dbContext.Notes.ToListAsync();
                return Notes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Notes> GetNotesByid(int Id)
        {
            try
            {
                Notes model = await dbContext.Notes.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Notes> UpdateNotes(Notes model)
            {
            try
            {
                Notes _entity = await dbContext.Notes.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                if(model.NotesTag!=null && model.NotesTag!="")
                {
                    _entity.NotesTag = model.NotesTag;
                }
                if(model.NotesDescripation!=_entity.NotesDescripation)
                {
                    _entity.NotesDescripation = model.NotesDescripation;
                }
                if(model.NotesTittle!=null && model.NotesTittle !="")
                {
                    _entity.NotesTittle = model.NotesTittle;
                }
               
                dbContext.Entry(_entity).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return _entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteNotes(int Id)
        {
            try
            {
                Notes model = await dbContext.Notes.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(model).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }





        public async Task<int> AddadminNotesTagAsync(NotesTag model)
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


                dbContext.Entry(model).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return model;
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


        public async Task<int> AddNotesDetails(Notes model)
        {
            try
            {
                model.CreatedDate = DateTime.Now.ToString("dd-MM-yyyyy");
                await dbContext.Notes.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Notes>> GetNotesDetails()
        {
            try
            {
                List<Notes> Notes = await dbContext.Notes.ToListAsync();
                return Notes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Notes> GetNotesDetailsByid(int Id)
        {
            try
            {
                Notes model = await dbContext.Notes.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Notes> UpdateNotesDetails(Notes model)
        {
            try
            {
                Notes _entity = await dbContext.Notes.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (model.NotesTag != null && model.NotesTag != "")
                {
                    _entity.NotesTag = model.NotesTag;
                }
                if (model.NotesDescripation != _entity.NotesDescripation)
                {
                    _entity.NotesDescripation = model.NotesDescripation;
                }
                if (model.NotesTittle != null && model.NotesTittle != "")
                {
                    _entity.NotesTittle = model.NotesTittle;
                }

                dbContext.Entry(_entity).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return _entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteNotesDetails(int Id)
        {
            try
            {
                Notes model = await dbContext.Notes.Where(x => x.Id == Id).FirstOrDefaultAsync();
                dbContext.Entry(model).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
