using Model;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface ICollaboratorInterface
    {
        public bool AddCollaborator(CollaboratorModel model, int userId);

        public List<CollaboratorEntity> ViewCollaborators(int noteId);

        public bool DeleteCollaborators(int noteId, string email);

    }
}
