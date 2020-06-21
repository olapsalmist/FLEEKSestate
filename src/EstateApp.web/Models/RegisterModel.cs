using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EstateApp.web.Models
{
    public class RegisterModel
    {

        [DisplayName("Name")]
        [Required]
        public string FullName {get; set;}

        /* required is used to enforce input of email and password
        the DisplayName attribute is used to display name for our elements
        on the web page*/
        [DisplayName("Email Address")]
        [Required]
        [EmailAddress]
        public string Email { get; set;}

        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string password {get; set;}

        [DisplayName("Confirm Password")]
        [Required]
        [Compare(nameof(password))] //this code is used to compare both the password and the confirmpassword
        public string ConfirmPassword {get; set;}
    
    }
}