using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLayer;
using CommonLayer.Models;
using ManagerLayer;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;


namespace FundooNotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : Controller
    {

        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;
        private readonly INotesManager manager;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<NotesController> logger;

        public NotesController(INotesManager manager, FundooDBContext context, IConfiguration configuration, IDistributedCache distributedCache, ILogger<NotesController> logger)
        {
            this.context = context;
            this.configuration = configuration;
            this.manager = manager;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        [Authorize]
        [HttpPost("AddNote")]
        public IActionResult AddNotes(NotesModel model)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var notes = manager.AddNotes(model, Convert.ToInt32(userId));

                logger.LogInformation(notes.ToString());
                return Ok(new ResponseModel<Notes> { Success = true, Message = "Notes added succesfully", Data = notes });
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<Notes> { Success = false, Message = "Failed to add notes"});
            }
        }


        [Authorize]
        [HttpGet]
        [Route("getnotes")]
        public IActionResult GetNotes()
        {
            string data = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            var response = manager.GetNotes(Convert.ToInt32(data));
            if (response != null)
            {
                return Ok(new ResponseModel<List<Notes>> { Success = true, Message = "notes retrieved", Data = response });
            }
            else
            {
                return BadRequest(new ResponseModel<List<Notes>> { Success = false, Message = "unable to retrieve" });
            }

        }

        [Authorize]
        [HttpPut("updatenotes")]
        public IActionResult UpdateNotes(int notesId, UpdateNotesModel model)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var result = manager.UpdateNotes(Convert.ToInt32(userId), notesId, model);
                logger.LogInformation(result.ToString());
                return Ok(new ResponseModel<Notes> { Success = true, Message = "Notes updated successfully", Data = result });
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.ToString());
                return Ok(new ResponseModel<Notes> { Success = false, Message = "failed to updated notes" });
            }
        }


        [Authorize]
        [HttpDelete("DeleteNotes")]
        public IActionResult Delete(int notesId, int userId)
        {
            try
            {
                bool result = manager.DeleteNotes(notesId, userId);
                logger.LogInformation(result.ToString());
                return Ok(new ResponseModel<bool> { Success = true, Message = "Notes deleted successfully" });
            }
            catch(Exception ex)
            {
                return Ok(new ResponseModel<bool> { Success = false, Message = "failed to deleted notes" });
            }

        }


        //[Authorize]
        //[HttpPut("ispinunpin")]
        //public IActionResult IsPinUnPin(int userId, int NotesId)
        //{
        //    var userClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");

        //    if (userClaim == null)
        //    {
        //        return Unauthorized(new ResponseModel<bool> { Success = false, Message = "invalid user authentication" });
        //    }
        //    userId = Convert.ToInt32(userClaim.Value);
        //    var result = manager.IsPinUnpin(userId, NotesId);
        //    if (result != null)
        //    {
        //        return Ok(new ResponseModel<bool> { Success = true, Message = "notes pin successfully", Data = result });
        //    }
        //    else
        //    {
        //        return BadRequest(new ResponseModel<bool> { Success = false, Message = "notes not pinned" });
        //    }
        //}

        [HttpGet]
        [Route("RedisGetallNotes")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "NotesList";
            string SerializeNotesList;
            var NoteList = new List<Notes>();
            var RedisNotesList = await distributedCache.GetAsync(cacheKey);
            if (RedisNotesList != null)
            {
                SerializeNotesList = Encoding.UTF8.GetString(RedisNotesList);
                NoteList = JsonConvert.DeserializeObject<List<Notes>>(SerializeNotesList);

            }
            else
            {
                NoteList = context.Notes.ToList();
                SerializeNotesList = JsonConvert.SerializeObject(NoteList);
                RedisNotesList = Encoding.UTF8.GetBytes(SerializeNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(20))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                await distributedCache.SetAsync(cacheKey, RedisNotesList, options);

            }
            return Ok(NoteList);
        }

        [Authorize]
        [HttpPut("color")]
        public IActionResult Color(int notesId, string color)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var result = manager.Color(Convert.ToInt32(userId), notesId, color);

                logger.LogInformation(result.ToString());

                return Ok(new ResponseModel<Notes> { Success = true, Message = "color set successful", Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString()); 
                return BadRequest(new ResponseModel<Notes> { Success = false, Message = "failed to set color" }); 
            }
        }


        [Authorize]
        [HttpPut("image")]
        public IActionResult Image(int notesId, IFormFile path)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var result = manager.Image(Convert.ToInt32(userId), notesId, path);

                logger.LogInformation(result.ToString()); 

                return Ok(new ResponseModel<Notes> { Success = true, Message = "image insert successful", Data = result });
            }
            catch (Exception ex) 
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<Notes> { Success = false, Message = "failed to insert image" }); 
            }
        }

        [Authorize]
        [HttpPut("reminder")]
        public IActionResult Remainder(int notesId, DateTime datetime)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var result = manager.Remainder(Convert.ToInt32(userId), notesId, datetime);

                logger.LogInformation(result.ToString()); 

                return Ok(new ResponseModel<Notes> { Success = true, Message = "remainder set successful", Data = result });
            }
            catch (Exception ex) 
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<Notes> { Success = false, Message = "failed to set remainder" }); 
            }
        }

        [Authorize]
        [HttpPut("pin")]
        public IActionResult Pin(int notesId)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var result = manager.PinUnPin(Convert.ToInt32(userId), notesId);
                logger.LogInformation(result.ToString());
                return Ok(new ResponseModel<bool> { Success = true, Message = "pin set successful", Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<bool> { Success = false, Message = "failed to set pin" });
            }
        }

        [Authorize]
        [HttpPut("trashuntrash")]
        public IActionResult Trash(int notesId)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var result = manager.TrashUnTrash(Convert.ToInt32(userId), notesId);

                logger.LogInformation(result.ToString());

                return Ok(new ResponseModel<bool> { Success = true, Message = "successfull", Data = result });

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<bool> { Success = false, Message = "un-success" });
            }
            
        }

        [Authorize]
        [HttpPut("archiveunarchive")]
        public IActionResult Archive(int notesId)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                var result = manager.Archive(Convert.ToInt32(userId), notesId);

                logger.LogInformation(result.ToString());

                return Ok(new ResponseModel<bool> { Success = true, Message = "successfull", Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<bool> { Success = false, Message = "un-success" });
            }
            
        }

        [Authorize]
        [HttpPut("deleteforever")]

        public IActionResult DeleteForever(int notesId)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                bool result = manager.DeleteForever(Convert.ToInt32(userId), notesId);

                logger.LogInformation(result.ToString());

                return Ok(new ResponseModel<bool> { Success = true, Message = "delete successful", Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<bool> { Success = false, Message = "failed to delete" });
            }
            
        }
    }
}