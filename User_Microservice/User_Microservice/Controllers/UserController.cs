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


        [HttpPost("userregistration")]
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

        [HttpPost("userlogin")]
        public ResponseModel<string> UserLogin(UserLoginModel model)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                var token = _userInterface.UserLogin(model);

                if (token != null)
                {
                    responseModel.Message = "User logged in successfully.";
                    responseModel.Data = token;
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "Wrong Credentials.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Success=false;
                responseModel.Message = ex.Message;
            }
            return responseModel;
        }


    }
}
