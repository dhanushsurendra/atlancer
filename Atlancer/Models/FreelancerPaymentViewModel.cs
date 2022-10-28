using Atlancer.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlancer.Models
{
    public class FreelancerPaymentViewModel
    {
        
        public IEnumerable<Freelancer> Freelancers { get; set; }
        public IEnumerable<Payment> Payment { get; set; }
       
    }
}
