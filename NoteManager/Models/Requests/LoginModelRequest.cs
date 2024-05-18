﻿using System.ComponentModel.DataAnnotations;

namespace NoteManager.Models.Requests
{
    public class LoginModelRequest
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
