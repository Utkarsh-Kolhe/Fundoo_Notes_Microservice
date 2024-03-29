
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Entity;
using System.Security.Claims;
using Interface;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorInterface _collaboratorInterface;

        public CollaboratorController(ICollaboratorInterface collaboratorInterface)
        {
            _collaboratorInterface = collaboratorInterface;
        }

        [HttpPost]
        [Authorize]
        public ResponseModel<CollaboratorModel> AddCollaborator(CollaboratorModel model)
        {
            var responseModel = new ResponseModel<CollaboratorModel>();

            var _userId = User.FindFirstValue("UserId");
            int userId = Convert.ToInt32(_userId);

            var result = _collaboratorInterface.AddCollaborator(model, userId);

            if(result == true)
            {
                responseModel.Message = "Collaborator added Successfully.";
                responseModel.Data = model;
            }
            else
            {
                responseModel.Success = false;
                responseModel.Message = "Error While Adding Collaborator! Please try again.";
            }
            return responseModel;
        }

        [HttpGet]
        [Authorize]
        public ResponseModel<List<CollaboratorEntity>> ViewCollaborators(int noteId)
        {
            var data = _collaboratorInterface.ViewCollaborators(noteId);

            ResponseModel<List<CollaboratorEntity>> responseModel = new ResponseModel<List<CollaboratorEntity>>();

            if(data.Count != 0)
            {
                responseModel.Message = "Collaborator email retrived successfully.";
                responseModel.Data = data;
            }
            else
            {
                responseModel.Success = false;
                responseModel.Message = "Error while retriving the collaborator email.";
            }

            return responseModel;
        }


        [HttpDelete]
        [Authorize]
        public ResponseModel<string> DeleteCollaborator(int noteId, string email)
        {
            var responseModel = new ResponseModel<string>();
            var data = _collaboratorInterface.DeleteCollaborators(noteId, email);
            if (data)
            {
                responseModel.Message = "Collaborator deleted successfully.";
                responseModel.Data = email;
            }
            else
            {
                responseModel.Success = false;
                responseModel.Message = "Error while deleteing the collaborator.";
            }

            return responseModel;
        }
    }
}
