using Atlancer.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlancer.Models
{
    public class ClientViewModel
    {
        public Client Client { get; set; }

        public List<Project> Project { get; set; }
        public int ReviewsCount { get; set; }
    }
}
