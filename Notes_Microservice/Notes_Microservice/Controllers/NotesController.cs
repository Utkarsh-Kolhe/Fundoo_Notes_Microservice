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

                if(result)
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
    }
}
