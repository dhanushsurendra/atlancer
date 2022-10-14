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
        public ActionResult Index(string? id)
        {

            if (id == "" || id == null)
            {
                return NotFound();
            }

            Globals.FreelancerId = id;
            ViewBag.UserId = Globals.UserId;
            ViewBag.UserType = "Client";
            ViewBag.UserName = Globals.UserName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Feedback feedback)
        {
            ModelState.Remove("FeedbackId");
            ModelState.Remove("ClientId");
            ModelState.Remove("Client");
            ModelState.Remove("FreelancerId");
            ModelState.Remove("Freelancer");

            ViewBag.UserType = Globals.UserType;

            var client = _db.Client.Find(Globals.UserId);
            var freelancer = _db.Freelancer.Find(Globals.FreelancerId);

            // alpha numeric - 10 characters
            var id = Guid.NewGuid().ToString().Replace("-", String.Empty).Substring(0, 10);

            feedback.FeedbackId = id;
            feedback.ClientId = Globals.UserId;
            feedback.FreelancerId = Globals.FreelancerId;

            if (client != null)
            {
                feedback.Client = client;
            }

            if (freelancer != null)
            {
                feedback.Freelancer = freelancer;
            }

            if (ModelState.IsValid)
            {
                _db.Feedback.Add(feedback);
                _db.SaveChanges();

                return RedirectToAction("Index", "Client", new { id = Globals.UserId });
            }

            return View();
        }

        // GET: FeedbackController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FeedbackController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeedbackController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FeedbackController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FeedbackController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FeedbackController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FeedbackController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
