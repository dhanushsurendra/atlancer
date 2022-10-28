using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Atlancer.Controllers
{
    public class ClientController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ClientController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string? id)
        {

            ClientViewModel clientViewModel = new ClientViewModel();
            ModelState.Remove("Client");
            ModelState.Remove("Project");
            ModelState.Remove("ReviewsCount");

            if (id == "" || id == null)
            {
                return NotFound();
            }

            var reviewsCount = _db.Feedback.ToList().Count();
            clientViewModel.ReviewsCount = reviewsCount;

            var client = _db.Client.Find(id);

            if (client != null)
            {
                clientViewModel.Client = client;
            }


            var cId = new SqlParameter("@id", id);
            List<Project> projects = _db.Project.FromSqlRaw("SELECT * FROM Project WHERE ClientId=@id", cId).ToList();

            if (projects == null)
            {
                return NotFound();
            }

            clientViewModel.Project = projects;

            ViewBag.UserType = "Client";
            ViewBag.UserName = client?.ClientName;
            ViewBag.UserId = client?.ClientId;

            return View(clientViewModel);
        }

        public IActionResult Create(string email, string password)
        {
            TempData["email"] = email;
            TempData["password"] = password;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {

            // remove the properties and assign value
            ModelState.Remove("ProfileImageName");
            ModelState.Remove("ClientId");

            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(client.ProfileImage.FileName);
            string extension = Path.GetExtension(client.ProfileImage.FileName);
            fileName += DateTime.Now.ToString("yymmssffff") + extension;
            client.ProfileImageName = fileName;
            string path = Path.Combine(wwwRootPath, "uploads/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await client.ProfileImage.CopyToAsync(fileStream);
            }

            // alpha numeric - 10 characters
            var id = Guid.NewGuid().ToString().Replace("-", String.Empty).Substring(0, 10);

            client.ClientId = id;

            if (ModelState.IsValid)
            {
                // save image to wwwroot/uploads/
                _db.Client.Add(client);
                _db.SaveChanges();

                Globals.UserId = id;
                Globals.UserType = Globals.UserTypes.Client.ToString();

                return RedirectToAction("Index", "Home", new { userId = id, userType = Globals.UserTypes.Client.ToString() });
            }

            TempData["email"] = client.Email;
            TempData["password"] = client.Password;
            return View();

        }

        public ActionResult Edit(string? id)
        {
            if (id == null || id == "")
            {
                return NotFound();
            }

            var client = _db.Client.Find(id);

            if (client == null)
            {
                return NotFound();
            }

            Globals.ProfileImageName = client.ProfileImageName;
            Globals.Email = client.Email;
            Globals.Password = client.Password;

            ViewBag.UserId = client?.ClientId;
            ViewBag.UserType = "Freelancer";
            ViewBag.UserName = client?.ClientName;

            TempData["email"] = client?.Email;
            TempData["password"] = client?.Password;

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Client client)
        {

            ModelState.Remove("ProfileImageName");
            ModelState.Remove("Email");
            ModelState.Remove("Password");
            ModelState.Remove("ClientId");

            DeleteImage(Globals.ProfileImageName);

            if (client == null)
            {
                return NotFound();
            }

            client.ProfileImageName = await UploadImage(client.ProfileImage);
            client.Email = Globals.Email;
            client.Password = Globals.Password;
            client.ClientId = Globals.UserId;

            if (ModelState.IsValid)
            {
                _db.Client.Update(client);
                _db.SaveChanges();
                TempData["success"] = "Client updated successfully";
                return RedirectToAction("Index", "Client", new { id = Globals.UserId });
            }

            ViewBag.UserId = client?.ClientId;
            ViewBag.UserType = "Freelancer";
            ViewBag.UserName = client?.ClientName;

            TempData["email"] = client?.Email;
            TempData["password"] = client?.Password;

            return View(client);
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            fileName += DateTime.Now.ToString("yymmssffff") + extension;
            string path = Path.Combine(wwwRootPath, "uploads/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public bool DeleteImage(string image)
        {
            try
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = image;
                string path = Path.Combine(wwwRootPath, "uploads/", fileName);
                FileInfo file = new FileInfo(path);

                if (file.Exists)
                {
                    System.IO.File.Delete(path);
                    file.Delete();
                    return true;
                }
                else
                {
                    Debug.WriteLine("File does not exist.");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}


