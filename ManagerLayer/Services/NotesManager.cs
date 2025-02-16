using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class NotesManager : INotesManager
    {
        public readonly INotesRepository note;
        public readonly FundooDBContext context;

        public NotesManager(INotesRepository note, FundooDBContext context)
        {
            this.note = note;
            this.context = context;
        }

        public Notes AddNotes(NotesModel model, int UserId)
        {
            return note.AddNotes(model, UserId);
        }

        public List<Notes> GetNotes(int UserId)
        {
            return note.GetNotes(UserId);
        }

        public Notes UpdateNotes(int UserId, int NotesId, UpdateNotesModel model)
        {
            return note.UpdateNotes(UserId, NotesId, model);
        }

        public bool DeleteNotes(int NotesId, int UserId)
        {
            return note.DeleteNotes(NotesId, UserId);
        }

        //public int GetNotesCount(int UserId)
        //{
        //    return note.GetNotesCount(UserId);
        //}

        //public bool IsPinUnpin(int userId, int notesId)
        //{
        //    return note.IsPinUnpin(userId, notesId);
        //}

        //public Notes GetNotesCount()
        //{
        //    throw new NotImplementedException();
        //}

        //public NotesEntity GetNotesCount()
        //{
        //    throw new NotImplementedException();
        //}

        public bool PinUnPin(int userId, int notesId)
        {
            return note.PinUnPin(userId, notesId);
        }

        public Notes Image(int userId, int notesId, IFormFile path)
        {
            return note.Image(userId, notesId, path);
        }

        public bool DeleteForever(int userId, int notesId)
        {
            return note.DeleteForever(userId, notesId);
        }

        public bool Archive(int userId, int notesId)
        {
            return note.Archive(userId, notesId);
        }

        public bool TrashUnTrash(int userId, int notesId)
        {
            return note.TrashUnTrash(userId, notesId);
        }

        public Notes Color(int userId, int notesId, string color)
        {
            return note.Color(userId, notesId, color);
        }

        public Notes Remainder(int userId, int notesId, DateTime datetimeToReminder)
        {
            return note.Remainder(userId, notesId, datetimeToReminder);
        }
    }
}