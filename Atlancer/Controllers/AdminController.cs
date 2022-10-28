using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace Atlancer.Controllers
{
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {

            ViewBag.Projects = _db.Project.Count();
            ViewBag.PayoutReleased = await _db.Freelancer.SumAsync(f => f.Wallet);
            ViewBag.Revenue = _db.Admin.ToList().ElementAt(0).Revenue;
            ViewBag.PayoutPending = _db.Admin.Select(a => a.Wallet);
            ViewBag.TotalNoOfUsers = _db.Freelancer.Count() +  _db.Client.Count();
            ViewBag.Wallet = _db.Admin.Select(a => a.Wallet);
            ViewBag.Ongoing = _db.Project.Where(p => p.ProjectStatus == "Ongoing").Count();
            ViewBag.Completed = _db.Project.Where(p => p.ProjectStatus == "Completed").Count();
            ViewBag.Bidding = _db.Project.Where(p => p.ProjectStatus == "Bidding").Count();

            IEnumerable<Freelancer> freelancers = _db.Freelancer.ToList();
            IEnumerable<Payment> Payment = _db.Payment.ToList();

            FreelancerPaymentViewModel freelancerPaymentViewModel = new FreelancerPaymentViewModel();
            freelancerPaymentViewModel.Freelancers = freelancers;
            freelancerPaymentViewModel.Payment = Payment;

            return View(freelancerPaymentViewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var email = new SqlParameter("@email", admin.Email);
                var password = new SqlParameter("password", admin.Password);

                var exists = _db.Admin.FromSqlRaw($"SELECT * FROM Admin WHERE Email=@email AND Password=@password", email, password).Count();


                // check if the count is 0
                if (exists == 0)
                {
                    // invalid credentials
                    TempData["error"] = "Invalid credentials";
                    return View();
                } else
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
            
        }
    }
}
