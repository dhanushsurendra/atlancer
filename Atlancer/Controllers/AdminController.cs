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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            // find if the credentials are correct
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
