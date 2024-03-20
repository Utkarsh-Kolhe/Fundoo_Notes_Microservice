using Notes_Microservice.Entity;
using Notes_Microservice.Models;

namespace Notes_Microservice.Interface
{
    public interface INotesInterface
    {
        public bool AddNote(NotesModel model, int userId);
        public List<NotesEntity> GetNotes(int userId);
        public bool EditNote(int noteId, int userId, NotesModel model);
        public bool DeleteNote(int noteId, int userId);
    }
}
