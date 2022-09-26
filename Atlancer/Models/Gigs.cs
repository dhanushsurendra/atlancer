using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Atlancer.Models;
using System.ComponentModel;

namespace Atlancer.Models
{
    public class Gigs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string GigId { get; set; }

        public string FreelancerId { get; set; }

        [ForeignKey("FreelancerId")]
        public virtual Freelancer Freelancer { get; set; }

        [DisplayName("Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Required(ErrorMessage = "Price is required")]
        public float Price { get; set; } = 0;

        [DisplayName("Title")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required]
        public string DeliveryTime { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [NotMapped]
        [Display(Name = "Profile Image 1")]
        public IFormFile GigImage1 { get; set; }

        public string GigImageName1 { get; set; }

        [NotMapped]
        [Display(Name = "Profile Image 2")]
        public IFormFile GigImage2 { get; set; }

        public string GigImageName2 { get; set; }

        [NotMapped]
        [Display(Name = "Profile Image 3")]
        public IFormFile GigImage3 { get; set; }

        public string GigImageName3 { get; set; }
    }
}
