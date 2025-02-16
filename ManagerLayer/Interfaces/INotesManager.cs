using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface INotesManager
    {
        public Notes AddNotes(NotesModel model, int UserId);

       // public int GetNotesCount(int UserId);

        public List<Notes> GetNotes(int UserId);

        public Notes UpdateNotes(int UserId, int NotesId, UpdateNotesModel model);

        public bool DeleteNotes(int NotesId, int UserId);

        public bool PinUnPin(int userId, int notesId);

        public Notes Image(int userId, int notesId, IFormFile path);

        public bool DeleteForever(int userId, int notesId);

        public bool Archive(int userId, int notesId);

        public bool TrashUnTrash(int userId, int notesId);

        public Notes Color(int userId, int notesId, string color);

        public Notes Remainder(int userId, int notesId, DateTime datetimeToReminder);


    }
}
