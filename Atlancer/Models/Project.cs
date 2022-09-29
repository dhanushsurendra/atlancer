using Atlancer.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlancer.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ProjectId { get; set; }

        [DisplayName("Project Name")]
        [Required(ErrorMessage = "Name is required")]
        public string ProjectName { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [DisplayName("Budget")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Required(ErrorMessage = "Budget is required")]
        public float Budget { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public DateTime PostingDate { get; set; } = DateTime.Now;
        public DateTime BidEndDate { get; set; } = DateTime.Now.AddDays(3);

        // ongoing, completed
        public string ProjectStatus { get; set; } = "Bidding";
        public string ProjectTime { get; set; }
        public string ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
