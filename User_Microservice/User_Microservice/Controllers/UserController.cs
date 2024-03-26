using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User_Microservice.Entity;
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

        [HttpGet]
        [Authorize]
        public ResponseModel<UserDisplayModel> GetUserDetails()
        {
            var responseModel = new ResponseModel<UserDisplayModel>();

            try
            {
                var _id = User.FindFirstValue("UserId");
                int id = Convert.ToInt32(_id);

                var user = _userInterface.GetUserById(id);

                if(user != null)
                {
                    responseModel.Message = "User details retrived successfully.";
                    responseModel.Data = user;
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "User Not Found.";
                }
            }
            catch(Exception ex)
            {
                responseModel.Success=false;
                responseModel.Message = ex.Message;
            }
            return responseModel;
        }


    }
}
