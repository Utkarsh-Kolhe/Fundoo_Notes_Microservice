﻿using System.ComponentModel.DataAnnotations;

namespace User_Microservice.Model
{
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
