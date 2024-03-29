using Model;
using Context;
using Entity;
using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CollaboratorService : ICollaboratorInterface
    {
        private readonly CollaboratorContext _collaboratorContext;

        public CollaboratorService(CollaboratorContext collaboratorContext)
        {
            _collaboratorContext = collaboratorContext;
        }

        public bool AddCollaborator(CollaboratorModel model, int userId)
        {
            CollaboratorEntity collabratorEntity = new CollaboratorEntity();
            collabratorEntity.Collaborator_Email = model.Email;
            collabratorEntity.NoteId = model.NoteId;
            collabratorEntity.UserId = userId;

            _collaboratorContext.Add(collabratorEntity);
            _collaboratorContext.SaveChanges();
            return true;
        }

        public List<CollaboratorEntity> ViewCollaborators(int noteId)
        {
            List<CollaboratorEntity> collaboratorList = _collaboratorContext.Collaborators.Where(e =>  e.NoteId == noteId).ToList();
            return collaboratorList;
        }

        public bool DeleteCollaborators(int noteId, string email)
        {
            var data = _collaboratorContext.Collaborators.FirstOrDefault(e => e.NoteId == noteId && e.Collaborator_Email == email);
            if (data != null)
            {
                _collaboratorContext.Collaborators.Remove(data);
                _collaboratorContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
