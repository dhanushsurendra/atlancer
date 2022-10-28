using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Atlancer.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FeedbackController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: FeedbackController1
        public ActionResult Create(string? id)
        {

            if (id == "" || id == null)
            {
                return NotFound();
            }

            Console.WriteLine(Globals.FreelancerId);
            ViewBag.UserId = Globals.UserId;
            ViewBag.UserType = "Client";
            ViewBag.UserName = Globals.UserName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feedback feedback)
        {
            ModelState.Remove("FeedbackId");
            ModelState.Remove("ClientId");
            ModelState.Remove("Client");

            ViewBag.UserType = Globals.UserType;

            var client = _db.Client.Find(Globals.UserId);
            var freelancer = _db.Freelancer.Find(Globals.FreelancerId);

            // alpha numeric - 10 characters
            var id = Guid.NewGuid().ToString().Replace("-", String.Empty).Substring(0, 10);

            feedback.FeedbackId = id;
            feedback.ClientId = Globals.UserId;

            if (client != null)
            {
                feedback.Client = client;
            }

            if (ModelState.IsValid)
            {
                _db.Feedback.Add(feedback);
                _db.SaveChanges();

                return RedirectToAction("Index", "Client", new { id = Globals.UserId });
            }

            return View();
        }
    }
}
