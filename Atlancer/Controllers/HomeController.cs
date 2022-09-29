using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Atlancer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(string userId, string userType)
        {
            if (userType == Globals.UserTypes.Freelancer.ToString())
            {
                var freelancer = _db.Freelancer.Find(userId);
                IEnumerable<Project> projectsList = _db.Project;

                ViewBag.UserType = "Freelancer";
                ViewBag.UserName = freelancer?.FreelancerName;
                ViewBag.UserId = freelancer?.FreelancerId;
                return View(projectsList);
            }

            var client = _db.Client.Find(userId);
            ViewBag.UserType = "Client";
            ViewBag.UserName = client?.ClientName;
            ViewBag.UserId = client?.ClientId;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}