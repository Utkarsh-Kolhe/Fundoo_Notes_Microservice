using Notes_Microservice.Context;
using Notes_Microservice.Entity;
using Notes_Microservice.Interface;
using Notes_Microservice.Models;

namespace Notes_Microservice.Service
{
    public class NotesService : INotesInterface
    {
        private readonly NotesContext _context;

        public NotesService(NotesContext context)
        {
            _context = context;
        }
        public bool AddNote(NotesModel model, int userId)
        {
            NotesEntity notesEntity = new NotesEntity();
            notesEntity.Title = model.Title;
            notesEntity.Description = model.Description;
            notesEntity.Colour = model.Colour;
            notesEntity.CreatedBy = userId;

            _context.Notes.Add(notesEntity);
            _context.SaveChanges();

            return true;
        }

        public List<NotesEntity> GetNotes(int userId)
        {
            var notes = _context.Notes.Where(e => e.CreatedBy == userId).ToList();
            return notes;
        }
    }
}
