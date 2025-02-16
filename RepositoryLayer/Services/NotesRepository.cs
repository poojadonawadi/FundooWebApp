using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;


namespace RepositoryLayer.Services
{
    public class NotesRepository : INotesRepository
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;

        public NotesRepository(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public Notes AddNotes(NotesModel model, int UserId)
        {
            Notes notes = new Notes();
            notes.Title = model.Title;
            notes.Description = model.Description;
            notes.UserId = UserId;
            notes.CreateAt = DateTime.Now;
            notes.color = model.color;
            notes.image = model.image;
            notes.IsArchive = model.IsArchive;
            notes.IsTrash = model.IsTrash;
            notes.UpdateAt = model.UpdateAt;
            context.Add(notes);
            context.SaveChanges();
            return notes;
        }

        public List<Notes> GetNotes(int UserId)
        {
            var listOfNotes = context.Notes.Where(x => x.UserId == UserId).ToList().ToList();
            return listOfNotes;
        }

        public Notes UpdateNotes(int UserId, int NotesId, UpdateNotesModel model)
        {
            var notes = context.Notes.FirstOrDefault(x => x.UserId == UserId && x.NotesId == NotesId);
            if (notes == null)
            {
                return null;
            }
            notes.Title = model.Title;
            notes.Description = model.Description;
            notes.Reminder = model.Reminder;
            notes.color = model.Colour;
            notes.IsPin = model.IsPin;
            notes.IsArchive = model.IsArchive;
            notes.IsTrash = model.IsTrash;
            notes.UpdateAt = DateTime.Now;

            context.SaveChanges();

            return notes;
        }

        public bool DeleteNotes(int UserId, int NotesId)
        {
            var notes = context.Notes.FirstOrDefault(x => x.UserId == UserId && x.NotesId == NotesId);
            if (notes == null)
            {
                return false;
            }

            context.Notes.Remove(notes);
            context.SaveChanges();
            return true;
        }


        public bool PinUnPin(int userId, int notesId)
        {
            var checkPin = context.Notes.FirstOrDefault(x => x.NotesId == notesId && x.UserId == userId);

            if (checkPin != null)
            {
                if (checkPin.IsPin == true)
                {
                    checkPin.IsPin = false;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    checkPin.IsPin = true;
                    context.SaveChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public Notes Image(int userId, int notesId, IFormFile path)
        {
            try
            {
                var data = context.Notes.FirstOrDefault(x => x.UserId == userId && x.NotesId == notesId);
                if (data != null)
                {
                    data.image = UploadImage(path, notesId, userId);
                    data.UpdateAt = DateTime.Now;
                    context.SaveChanges();
                    
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteForever(int userId, int notesId)
        {
            try
            {
                var notes = context.Notes.FirstOrDefault(x => x.NotesId == notesId && x.UserId == userId);
                if (notes != null)
                {
                    if(notes.IsTrash==true)
                    context.Notes.Remove(notes);
                    context.SaveChanges();
                    return true;
                }
                return false; 
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public bool Archive(int userId, int notesId)
        {
            var checkPin = context.Notes.FirstOrDefault(x => x.NotesId == notesId && x.UserId == userId);

            if (checkPin != null)
            {
                if (checkPin.IsArchive == true)
                {
                    checkPin.IsArchive = false;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    checkPin.IsArchive = true;
                    context.SaveChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool TrashUnTrash(int userId, int notesId)
        {
            var checkPin = context.Notes.FirstOrDefault(x => x.NotesId == notesId && x.UserId == userId);

            if (checkPin != null)
            {
                if (checkPin.IsTrash == true)
                {
                    checkPin.IsTrash = false;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    checkPin.IsTrash = true;
                    context.SaveChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public Notes GetNotesById(int UserId, int notesId)
        {
            Notes note = context.Notes.FirstOrDefault(x => x.UserId == UserId && x.NotesId == notesId);
            if(note != null)
            {
                return note;
            }
            return null;
        }

        public string UploadImage(IFormFile imagePath, int notesId, int userId)
        {
            var user = context.Users.Any(x => x.UserId == userId); 
            if (user) 
            {
                var note = GetNotesById(userId, notesId);
                if (note != null)
                {
                    Account account = new Account(
                        configuration["Cloudinary:CloudName"],
                        configuration["Cloudinary:ApiKey"],
                        configuration["Cloudinary:ApiSecret"] 
                    );

                    Cloudinary cloudinary = new Cloudinary(account);

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(imagePath.FileName, imagePath.OpenReadStream()),
                        PublicId = note.Title 
                    };

                    var uploadResult = cloudinary.Upload(uploadParams); 

                    if (uploadResult != null)
                    {
                        note.UpdateAt = DateTime.Now;
                        note.image = uploadResult.Url.ToString(); 
                        context.SaveChanges();
                        return uploadResult.Url.ToString();
                    }
                }
            }

            return null; 
        }

        public Notes Color(int userId, int notesId, string color)
        {
            try
            {
                var data = context.Notes.FirstOrDefault(x => x.UserId == userId && x.NotesId == notesId);
                if (data != null)
                {
                    data.color = color;
                    data.UpdateAt = DateTime.Now;
                    context.SaveChanges();
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Notes Remainder(int userId, int notesId, DateTime datetimeToReminder)
        {
            try
            {
                var data = context.Notes.FirstOrDefault(x => x.UserId == userId && x.NotesId == notesId);
                if (data != null)
                {
                    data.Reminder = datetimeToReminder;
                    data.UpdateAt = DateTime.Now;
                    context.SaveChanges();
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
    }
}