using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Atlancer.Models
{
    public class SignUp
    {
        // annotations for email
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        // annotations for password
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Enter a valid password")]
        public string Password { get; set; }

        [BindProperty]
        public UserType UserType { get; set; } = UserType.Freelancer;
    }

    public enum UserType {
        Freelancer,
        Client
    }
}
