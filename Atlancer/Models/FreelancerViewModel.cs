using Atlancer.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlancer.Models
{
    public class FreelancerViewModel
    {
        public Freelancer Freelancer { get; set; }

        public List<Gigs> Gigs { get; set; }
    }
}
