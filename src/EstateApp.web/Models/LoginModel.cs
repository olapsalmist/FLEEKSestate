using System;
using System.ComponentModel.DataAnnotations;

namespace EstateApp.web.Models
{
    public class LoginModel
    {

        // required is used to enforce input of email and password
        [Required]
        [EmailAddress]
        public string Email { get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string password {get; set;}
    }
}