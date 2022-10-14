using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;

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

            Globals.ProjectId = id;

            BidViewModel bidViewModel = new BidViewModel();
            ModelState.Remove("BidFreelancerList");
            ModelState.Remove("Project");

            var project = _db.Project.Find(id);

            if (project != null)
            {
                bidViewModel.Project = project;
            }

            // check if the user can place a bid, if not disable the button 
            var freelancerId = new SqlParameter("@id", Globals.UserId);
            var bidexists = _db.Bid.FromSqlRaw("SELECT * FROM Bid WHERE FreelancerId=@id", freelancerId).Count();
            if (bidexists > 0)
            {
                ViewBag.Disable = true;
            }

            var freelancer = _db.Freelancer.Find(id);

            var bids = (from bid in _db.Bid
                        join free in _db.Freelancer on bid.FreelancerId equals free.FreelancerId
                        where bid.ProjectId == id
                        select new { bid, free }
            ).ToList();

            List<Freelancer> freelancers = _db.Freelancer.ToList();

            BidFreelancerList bidfreelancerlist = new BidFreelancerList();

            foreach (var bid in bids)
            {
                BidFreelancerViewModel bidFreelancerViewModel = new BidFreelancerViewModel();
                bidFreelancerViewModel.Freelancer = bid.free;
                bidFreelancerViewModel.Bid = bid.bid;
                if (Globals.UserId == bid.free.FreelancerId)
                {
                    ViewBag.bidId = bid.bid.BidId;
                }
                bidfreelancerlist.BidFreelancerModel.Add(bidFreelancerViewModel);
            }

            bidViewModel.BidFreelancerList = bidfreelancerlist;

            if (freelancer != null)
            {
                ViewBag.UserName = freelancer.FreelancerName;
            }

            ViewBag.UserType = Globals.UserType;
            ViewBag.UserName = Globals.UserName;
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

        public IActionResult Edit(string? id)
        {
            if (id == "" || id == null)
            {
                return NotFound();
            }

            Globals.BidId = id;

            var bid = _db.Bid.Find(id);

            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Bid bid)
        {
            ModelState.Remove("FreelancerId");
            ModelState.Remove("ProjectId");
            ModelState.Remove("Freelancer");
            ModelState.Remove("Project");
            ModelState.Remove("BidId");

            bid.BidId = Globals.BidId;
            bid.FreelancerId = Globals.UserId;
            bid.ProjectId = Globals.ProjectId;

            var freelancer = _db.Freelancer.Find(Globals.UserId);
            var project = _db.Project.Find(Globals.ProjectId);

            if (freelancer != null)
            {
                bid.Freelancer = freelancer;
            }

            if (project != null)
            {
                bid.Project = project;
            }


            if (ModelState.IsValid)
            {
                _db.Bid.Update(bid);
                _db.SaveChanges();
                TempData["success"] = "Bid updated successfully";
                return RedirectToAction("Index", "Bid", new { id = Globals.ProjectId });
            }


            return View(bid);
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
