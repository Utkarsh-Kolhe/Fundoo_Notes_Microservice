using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_Microservice.Interface;
using User_Microservice.Model;

namespace User_Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userInterface;

        public UserController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }


        [HttpPost]
        public ResponseModel<UserRegistrationModel> UserRegistration(UserRegistrationModel model)
        {
            ResponseModel<UserRegistrationModel> responseModel = new ResponseModel<UserRegistrationModel>();

            try
            {
                bool result = _userInterface.AddNewUser(model);

                if (result)
                {
                    responseModel.Message = "User Registered successfully.";
                    responseModel.Data = model;
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "User already exist.";
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
