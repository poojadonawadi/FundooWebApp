using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface ICollaboratorManager
    {
        public Collaborators AddCollaborator(CollaboratorModel model, int userId);

        public List<Collaborators> GetAllCollaborators(int userId);

        public bool DeleteCollaborator(int collaboratorId, int notesId, int userId);

        public bool MailExists(string email);
    }
}
