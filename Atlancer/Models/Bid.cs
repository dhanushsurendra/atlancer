using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace Atlancer.Models
{
    public class Bid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string BidId { get; set; }

        public string ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public string FreelancerId { get; set; }

        [ForeignKey("FreelancerId")]
        public virtual Freelancer Freelancer { get; set; }

        public DateTime BidDate { get; set; }  = DateTime.Now;

        [Required(ErrorMessage = "Bid Amount is required")]
        public int BidAmount { get; set; }

        public double ServiceFree { get; set; } = 0.0;
        public double AmountPaid { get; set; } = 0.0;

        [Required(ErrorMessage = "Cover Letter is required")]
        public string CoverLetter { get; set; } 
    }
}
