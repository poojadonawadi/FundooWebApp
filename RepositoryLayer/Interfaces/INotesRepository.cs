using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepository
    {
        public Notes AddNotes(NotesModel model, int UserId);

        public List<Notes> GetNotes(int UserId);

        public Notes UpdateNotes(int UserId, int NotesId, UpdateNotesModel model);

        public bool DeleteNotes(int UserId, int NotesId);

        //public int GetNotesCount(int UserId);

        //public bool IsPinUnpin(int UserId, int notesId);

        //public bool TrashNote(int UserId, int notesId);


        //public bool DeleteNotesForever(int UserId, int NotesId);

        //public Notes AddReminder(int UserId, int NotesId, DateTime reminderTime);

        //public string AddColorToNote(int UserId, int NotesId, string color);

        //public string Image(string image, int notesId, int userId);

        public bool PinUnPin(int userId, int notesId);

        public Notes Image(int userId, int notesId, IFormFile path);

        public bool DeleteForever(int userId, int notesId);

        public bool Archive(int userId, int notesId);

        public bool TrashUnTrash(int userId, int notesId);

        public Notes Color(int userId, int notesId, string color);

        public Notes Remainder(int userId, int notesId, DateTime datetimeToReminder);

    }
}