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

        public bool EditNote(int noteId, int userId, NotesModel model)
        {
            var note = _context.Notes.FirstOrDefault(e => e.NoteId == noteId && e.CreatedBy == userId);
            if (note != null)
            {
                note.Title = model.Title;
                note.Description = model.Description;
                note.Colour = model.Colour;

                _context.SaveChanges();

                return true;
            }
            return false;
        }

        public bool DeleteNote(int noteId, int userId)
        {
            var note = _context.Notes.FirstOrDefault(e => (e.NoteId == noteId && e.CreatedBy == userId));
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
