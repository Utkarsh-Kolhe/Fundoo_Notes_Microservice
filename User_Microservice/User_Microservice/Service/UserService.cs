﻿using User_Microservice.Context;
using User_Microservice.Entity;
using User_Microservice.Interface;
using User_Microservice.Model;
using User_Microservice.Hashing;
using User_Microservice.JWT;
using Service;

namespace User_Microservice.Service
{
    public class UserService : IUserInterface
    {
        private readonly UserContext _context;
        private readonly HashingPassword _password;
        private readonly IConfiguration _configuration;

        public UserService(UserContext context, HashingPassword password, IConfiguration configuration)
        {
            _context = context;
            _password = password;
            _configuration = configuration;
        }

        public bool AddNewUser(UserRegistrationModel model)
        {
            var user = _context.User_Details.FirstOrDefault(e => e.Email == model.Email);
            if (user == null)
            {
                UserEntity userEntity = new UserEntity();

                userEntity.Email = model.Email;
                userEntity.FirstName = model.FirstName;
                userEntity.LastName = model.LastName;
                userEntity.Password = _password.HashPassword(model.Password);
                _context.User_Details.Add(userEntity);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string UserLogin(UserLoginModel model)
        {
            var user = _context.User_Details.FirstOrDefault(e => e.Email == model.Email);
            if (user != null)
            {
                bool passVerify = _password.VerifyPassword(model.Password, user.Password);
                if (passVerify)
                {
                    JwtToken token = new JwtToken(_configuration);
                    return token.GenerateToken(user);
                }
            }
            return null;
        }

        public UserDisplayModel GetUserById(int id)
        {
            var user = _context.User_Details.FirstOrDefault(_e => _e.Id == id);

            if (user != null)
            {
                UserDisplayModel model = new UserDisplayModel();
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Email = user.Email;
                return model;
            }
            return null;
        }

        public async Task<string?> Forget_Password(string email)
        {
            var user = _context.User_Details.FirstOrDefault(e => e.Email == email);
            JwtToken token = new JwtToken(_configuration);


            if (user != null)
            {
                string _token = token.GenerateTokenReset(user.Email, user.Id);


                // Generate password reset link
                var callbackUrl = $"https://localhost:7257/api/User/ResetPassword?token={_token}";

                // Send password reset email
                EmailService _emailService = new EmailService();
                await _emailService.SendEmailAsync(email, "Reset Password", callbackUrl);
                return "Ok";
            }
            else
            {
                return null;

            }
        }

        public bool PasswordReset(string newPassword, int userId)
        {
            var User = _context.User_Details.FirstOrDefault(e => e.Id == userId);

            if (User != null)
            {
                string newPass = _password.HashPassword(newPassword);
                User.Password = newPass;
                _context.SaveChanges();
                return true;
            }
            return false;


        }
    }
}
