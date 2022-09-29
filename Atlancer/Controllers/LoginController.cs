using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Atlancer.Controllers
{
    public class LoginController : Controller
    {

        private readonly ApplicationDbContext _db;

        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SignUp signUp)
        {
            if (ModelState.IsValid)
            {
                var email = new SqlParameter("@email", signUp.Email);
                var password = new SqlParameter("password", signUp.Password);

                if (signUp.UserType.ToString() == "Freelancer")
                {
                    var freelancer = _db.Freelancer.FromSqlRaw($"SELECT * FROM Freelancer WHERE Email=@email AND Password=@password", email, password).ToList();
                    // check if the count is 0
                    if (freelancer.Count == 0)
                    {
                        // invalid credentials
                        TempData["error"] = "Invalid credentials";
                        return View();
                    }
                    else
                    {
                        Globals.UserId = freelancer.ElementAt(0).FreelancerId;
                        Globals.UserType = "Freelancer";
                        Globals.UserName = freelancer.ElementAt(0).FreelancerName;
                        return RedirectToAction("Index", "Home", new { userId = freelancer?.ElementAt(0).FreelancerId, userType = "Freelancer" });
                    }
                }
                else
                {
                    var client = _db.Client.FromSqlRaw($"SELECT * FROM Client WHERE Email=@email AND Password=@password", email, password).ToList();

                    // check if the count is 0
                    if (client.Count == 0)
                    {
                        // invalid credentials
                        TempData["error"] = "Invalid credentials";
                        return View();
                    } else
                    {
                        Globals.UserId = client.ElementAt(0).ClientId;
                        Globals.UserType = "Client";
                        Globals.UserName = client.ElementAt(0).ClientName;
                        return RedirectToAction("Index", "Home", new { userId = client?.ElementAt(0).ClientId, userType = "Client" });
                    }
                }
            }
            return View();
        }
    }
}
