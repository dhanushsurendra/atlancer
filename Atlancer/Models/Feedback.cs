using Atlancer.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlancer.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public string FeedbackId { get; set; }

        public string ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public string FreelancerId { get; set; }

        [ForeignKey("FreelancerId")]
        public virtual Freelancer Freelancer { get; set; }

        [DisplayName("Title")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5)]
        public float Rating { get; set; } = 1;
    }
}
