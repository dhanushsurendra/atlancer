namespace Atlancer.Models
{
    public class BidFreelancerList
    {
        public List<BidFreelancerViewModel> BidFreelancerModel { get; set; }

        // initialize the list
        public BidFreelancerList()
        {
            BidFreelancerModel = new List<BidFreelancerViewModel>();
        }
    }
}

