using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _config;

        public UserController(IUserInterface userInterface, IConfiguration config)
        {
            _userInterface = userInterface;
            _config = config;
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

        [HttpPost("forgetpassword")]

        public async Task<ResponseModel<string>> ForgotPassword(string email)
        {
            var response = new ResponseModel<string>();

            var result = await _userInterface.Forget_Password(email);

            if (result != null)
            {
                response.Success = true;
                response.Message = "Reset password link sent successfully to your email address " + result;
            }
            else
            {
                response.Success = false;
                response.Message = "Unexpected error Occured ,Please Try again";
            }
            return response;
        }

        [HttpPost("resetpassword")]

        public ResponseModel<bool> ResetPassword(string token, string password)
        {
            var response = new ResponseModel<bool>();

            try
            {
                // Validate token
                var handler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["JWT:Issuer"],
                    ValidAudience = _config["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]))
                };

                SecurityToken validatedToken;
                var principal = handler.ValidateToken(token, validationParameters, out validatedToken);

                // Extract claims
                var userId = principal.FindFirstValue("UserId");
                int _userId = Convert.ToInt32(userId);

                // Perform operation (reset password) using userId
                // Note: Replace this with your actual password reset logic
                var result = _userInterface.PasswordReset(password, _userId);

                if (result)
                {
                    response.Success = true;
                    response.Message = "Password reset successful";
                    response.Data = result;
                }
                else
                {
                    response.Success = false;
                    response.Message = "An unexpected error occurred. Please try again.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error resetting password: " + ex.Message;
            }

            return response;
        }
    }
}
