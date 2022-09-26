using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Atlancer.Controllers
{
    public class BidController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BidController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string? id)
        {

            if (id == "" || id == null)
            {
                return NotFound();
            }

            BidViewModel bidViewModel = new BidViewModel();
            ModelState.Remove("BidFreelancerList");
            ModelState.Remove("Project");

            var project = _db.Project.Find(id);

            if (project != null)
            {
                bidViewModel.Project = project;
            }

            var bids = (from bid in _db.Bid
                      join freelan in _db.Freelancer on bid.FreelancerId equals freelan.FreelancerId
                      where bid.ProjectId == id
                      select new { bid, freelan } ).ToList();

            BidFreelancerList bidFreelancerList = new BidFreelancerList();

            foreach (var bid in bids)
            {
               BidFreelancerViewModel bidFreelancerViewModel = new BidFreelancerViewModel();
               bidFreelancerViewModel.Freelancer = bid.freelan;
               bidFreelancerViewModel.Bid = bid.bid;
               bidFreelancerList.BidFreelancerViewModel.Add(bidFreelancerViewModel); 
            }

            bidViewModel.BidFreelancerList = bidFreelancerList;

            ViewBag.UserType = "Freelancer";

            var freelancer = _db.Freelancer.Find(id);

            if (freelancer != null)
            {
                ViewBag.UserName = freelancer.FreelancerName;
            }

            ViewBag.UserId = Globals.UserId;

            return View(bidViewModel);
        }

        public IActionResult Create(string? id)
        {

            if (id == "" || id == null)
            {
                return NotFound();
            }

            ViewBag.UserType = Globals.UserType;
            Globals.ProjectId = id;
            
            if (Globals.UserType == "Freelancer")
            {
                ViewBag.UserName = _db.Freelancer.Find(Globals.UserId)?.FreelancerName;
                return View();
            }

            ViewBag.UserName = _db.Client.Find(Globals.UserId)?.ClientName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Bid bid)
        {

            ModelState.Remove("FreelancerId");
            ModelState.Remove("ProjectId");
            ModelState.Remove("Freelancer");
            ModelState.Remove("Project");
            ModelState.Remove("ProjectId");
            ModelState.Remove("BidId");

            ViewBag.UserType = Globals.UserType;
            bid.ProjectId = Globals.ProjectId;
            bid.FreelancerId = Globals.UserId;

            var freelancer = _db.Freelancer.Find(Globals.UserId);
            var project = _db.Project.Find(Globals.ProjectId);

            // alpha numeric - 10 characters
            var id = Guid.NewGuid().ToString().Replace("-", String.Empty).Substring(0, 10);

            bid.BidId = id;

            if (freelancer != null)
            {
                ViewBag.UserName = freelancer.FreelancerName;
                bid.Freelancer = freelancer;
            }

            if (project != null)
            {
                bid.Project = project;
            }

            if (ModelState.IsValid)
            {
                _db.Bid.Add(bid);
                _db.SaveChanges();

                return RedirectToAction("Index", "Bid", new { id = project?.ProjectId });

            }

            return View();
        }
    }
}
