using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Atlancer.Controllers
{
    public class PaymentController : Controller
    {

        private readonly ApplicationDbContext _db;

        public PaymentController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string? freelancerId)
        {

            if (freelancerId == "" || freelancerId == null)
            {
                return NotFound();
            }

            Globals.FreelancerId = freelancerId;

            ProjectViewModel projectViewModel = new ProjectViewModel();

            var freelancer = _db.Freelancer.Find(freelancerId);
            var projectId = new SqlParameter("@id", freelancerId);
            List<Bid> bid = _db.Bid.FromSqlRaw("SELECT * FROM Bid WHERE FreelancerId=@id", projectId).ToList();

            if (bid.Count > 0)
            {
                projectViewModel.Bid = bid.ElementAt(0);
                Globals.BidAmount = bid.ElementAt(0).BidAmount;
            }

            if (freelancer != null)
            {
                projectViewModel.Freelancer = freelancer;
            }

            var freeId = new SqlParameter("@id", freelancerId);
            List<Gigs> gig = _db.Gig.FromSqlRaw("SELECT * FROM Gig WHERE FreelancerId=@id", freeId).ToList();
            var project = _db.Project.Find(Globals.ProjectId);

            if (project != null)
            {
                projectViewModel.Project = project;
            }

            if (gig.Count > 0)
            {
                projectViewModel.Gig = gig.ElementAt(0);
            }

            ViewBag.UserId = Globals.UserId;
            ViewBag.UserType = "Client";
            ViewBag.UserName = Globals.UserName;


            return View(projectViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Index(Payment payment)
        {

            ModelState.Remove("PaymentId");
            ModelState.Remove("ClientId");
            ModelState.Remove("Client");
            ModelState.Remove("FreelancerId");
            ModelState.Remove("Freelancer");
            ModelState.Remove("BidAmount");
          
            // alpha numeric - 10 characters
            var id = Guid.NewGuid().ToString().Replace("-", String.Empty).Substring(0, 10);
            payment.PaymentId = id;
            payment.BidAmount = Globals.BidAmount;
            payment.ClientId = Globals.UserId;
            payment.FreelancerId = Globals.FreelancerId;

            var freelancer = _db.Freelancer.Find(Globals.FreelancerId);

            if (freelancer != null)
            {
                payment.Freelancer = freelancer;
            }

            var client = _db.Client.Find(Globals.UserId);

            if (client != null)
            {
                payment.Client = client;
            }

            if (ModelState.IsValid)
            {
                var revenue = 0.20 * payment.BidAmount;
                var wallet = payment.BidAmount - revenue;

                var revenueParam = new SqlParameter("@revenue", revenue);
                var walletParam = new SqlParameter("@wallet", wallet);
                var projectId = new SqlParameter("@projectId", Globals.ProjectId);

                _db.Database.ExecuteSqlRaw("UPDATE Admin SET Revenue=Revenue + @revenue, Wallet=Wallet + @wallet", revenueParam, walletParam);
                _db.Database.ExecuteSqlRaw("UPDATE Project SET ProjectStatus='Ongoing' WHERE ProjectId=@projectId", projectId);

                // add a new payment
                //_db.Payment.Add(payment);
                _db.SaveChanges();
                return RedirectToAction("Index", "Client", new { id = Globals.UserId });
            }
            return View(payment);
        }
    }
}
