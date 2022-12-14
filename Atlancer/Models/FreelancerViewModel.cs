using Atlancer.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlancer.Models
{
    public class FreelancerViewModel
    {
        public Freelancer Freelancer { get; set; }
        public List<Project> Projects { get; set; }
        public List<Gigs> Gigs { get; set; }
        public List<ClientFeedbackViewModel> Feedbacks { get; set; }

        // initialize the list
        public FreelancerViewModel()
        {
            Feedbacks = new List<ClientFeedbackViewModel>();
        }
    }
}
