using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        private readonly FundooDBContext context;

        public CollaboratorRepository(FundooDBContext context)
        {
            this.context = context;
        }

        public Collaborators AddCollaborator(CollaboratorModel model, int userId)
        {
            Collaborators collaborator = new Collaborators();
            collaborator.Email = model.Email;
            collaborator.NotesId = model.NotesId;
            collaborator.UserId = userId;
            context.Collaborators.Add(collaborator);
            context.SaveChanges();
            return collaborator;
        }

        public List<Collaborators> GetAllCollaborators(int userId)
        {
            var collaboratorList = context.Collaborators
                .Select(x => x)
              .Where(x => x.UserId == userId).ToList();

            if (collaboratorList != null)
            {
                return collaboratorList;
            }
            return null;
        }

        public bool DeleteCollaborator(int collaboratorId, int notesId, int userId)
        {
            var collaborator = context.Collaborators.FirstOrDefault(x => x.CollaboratorId == collaboratorId && x.NotesId == notesId && x.UserId == userId);

            if (collaborator != null)
            {
                context.Collaborators.Remove(collaborator);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}