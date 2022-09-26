using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Atlancer.Controllers
{
    public class SignUpController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SignUpController(ApplicationDbContext db)
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

                if (signUp.UserType.ToString() == "Freelancer")
                {
                    var exists = _db.Admin.FromSqlRaw($"SELECT * FROM Freelancer WHERE Email=@email", email).Count();

                    // check if the count is 0
                    if (exists == 0)
                    {   
                        return RedirectToAction("Create", "Freelancer", new { email = signUp.Email, password = signUp.Password });
   
                    }
                    else
                    {
                        // invalid credentials
                        TempData["error"] = "Email already exists";
                        return View();
                    }
                } else
                {
                    var exists = _db.Admin.FromSqlRaw($"SELECT * FROM Client WHERE Email=@email", email).Count();

                    // check if the count is 0
                    if (exists == 0)
                    {
                        return RedirectToAction("Create", "Client", new { email = signUp.Email, password = signUp.Password });
                    }
                    else
                    {
                        // invalid credentials
                        TempData["error"] = "Email already exists";
                        return View();
                    }
                }

                
            }
            return View();
        }
    }
}
