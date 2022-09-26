using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlancer.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ClientId { get; set; }

        [DisplayName("Freelancer Name")]
        [Required(ErrorMessage = "Name is required")]
        public string ClientName  { get; set; }

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

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Profile Image")]
        public string ProfileImageName { get; set; }

        [NotMapped]
        [Display(Name = "Profile Image")]
        public IFormFile ProfileImage { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
