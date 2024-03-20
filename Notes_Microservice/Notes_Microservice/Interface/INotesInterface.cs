using Notes_Microservice.Entity;
using Notes_Microservice.Models;

namespace Notes_Microservice.Interface
{
    public interface INotesInterface
    {
        public bool AddNote(NotesModel model, int userId);
        public List<NotesEntity> GetNotes(int userId);
    }
}
