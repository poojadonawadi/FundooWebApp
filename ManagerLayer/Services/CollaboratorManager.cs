using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class CollaboratorManager : ICollaboratorManager
    {
        public readonly ICollaboratorRepository collaborator;
        public readonly FundooDBContext context;

        public CollaboratorManager(ICollaboratorRepository collaborator, FundooDBContext context)
        {
            this.collaborator = collaborator;
            this.context = context;
        }

        public Collaborators AddCollaborator(CollaboratorModel model, int userId)
        {
            return collaborator.AddCollaborator(model, userId);
        }
        public List<Collaborators> GetAllCollaborators(int userId)
        {
            return collaborator.GetAllCollaborators(userId);
        }



        public bool DeleteCollaborator(int collaboratorId, int notesId, int userId)
        {
            return collaborator.DeleteCollaborator(collaboratorId, notesId, userId);
        }

        public bool MailExists(string email)
        {
            var checkEmail = this.context.Users.FirstOrDefault(x => x.Email == email);
            {
                if (checkEmail != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}