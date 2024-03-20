using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes_Microservice.Entity;
using Notes_Microservice.Interface;
using Notes_Microservice.Model;
using Notes_Microservice.Models;
using System.Security.Claims;

namespace Notes_Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesInterface _notesInterface;

        public NotesController(INotesInterface notesInterface)
        {
            _notesInterface = notesInterface;
        }

        [HttpPost]
        [Authorize]
        public ResponseModel<NotesModel> AddNote(NotesModel model)
        {
            ResponseModel<NotesModel> responseModel = new ResponseModel<NotesModel>();
            try
            {
                var _userId = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(_userId);

                var result = _notesInterface.AddNote(model, userId);

                if (result)
                {
                    responseModel.Message = "Note created successfully.";
                    responseModel.Data = model;
                }

            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = ex.Message;
            }
            return responseModel;
        }

        [HttpGet]
        [Authorize]
        public ResponseModel<List<NotesEntity>> GetNotes()
        {
            var responseModel = new ResponseModel<List<NotesEntity>>();
            try
            {
                var _userId = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(_userId);
                var notes = _notesInterface.GetNotes(userId);

                if (notes.Count != 0)
                {
                    responseModel.Message = "Notes retrived successfully.";
                    responseModel.Data = notes;
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "There is no note.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = ex.Message;
            }
            return responseModel;
        }

        [HttpPut]
        [Authorize]
        public ResponseModel<NotesModel> EditNote(int noteId, NotesModel model)
        {
            var responseModel = new ResponseModel<NotesModel>();
            try
            {
                var _userId = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(_userId);
                var check = _notesInterface.EditNote(noteId, userId, model);

                if (check)
                {
                    responseModel.Message = "Note edited successfully.";
                    responseModel.Data = model;
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "Note not found";
                }
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = ex.Message;
            }
            return responseModel;
        }

        [HttpDelete]
        [Authorize]
        public ResponseModel<string> DeleteNote(int noteId)
        {
            var responseModel = new ResponseModel<string>();
            try
            {
                var _userId = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(_userId);

                var result = _notesInterface.DeleteNote(noteId, userId);

                if(result)
                {
                    responseModel.Message = "Note delete successfully.";
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "Note not found";
                }
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = ex.Message;
            }

            return responseModel;
        }
    }
}
