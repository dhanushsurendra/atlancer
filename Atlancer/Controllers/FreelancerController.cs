﻿using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Atlancer.Controllers
{
    public class FreelancerController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FreelancerController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string? id)
        {

            if (id == "" || id == null)
            {
                return NotFound();
            }

            var fId = new SqlParameter("@id", id);
            List<Gigs> gigs = _db.Gig.FromSqlRaw("SELECT * FROM Gig WHERE FreelancerId=@id", fId).ToList();

            FreelancerViewModel freelancerViewModel = new FreelancerViewModel();
            ModelState.Remove("Gigs");
            ModelState.Remove("Freelancer");

            if (gigs != null)
            {
                freelancerViewModel.Gigs = gigs;
            }

            var freelancer = _db.Freelancer.Find(id);
            if (freelancer != null)
            {
                freelancerViewModel.Freelancer = freelancer;
            }

            ViewBag.UserType = "Freelancer";
            ViewBag.UserName = freelancer?.FreelancerName;
            ViewBag.UserId = freelancer?.FreelancerId;

            ViewBag.FreelancerId = id;
            ViewBag.FreelancerName = freelancer?.FreelancerName;
            ViewBag.MemberSince = freelancer?.MemberSince;
            ViewBag.Wallet = freelancer?.Wallet;
            ViewBag.Country = freelancer?.Country;
            ViewBag.ProfileImageName = freelancer?.ProfileImageName;
            ViewBag.Description = freelancer?.Description;
            ViewBag.PhoneNumber = freelancer?.PhoneNumber;
            ViewBag.Category = freelancer?.Category;

            if (freelancer == null)
            {
                return NotFound();
            }

            return View(freelancerViewModel);
        }

        public IActionResult Create(string email, string password)
        {
            TempData["email"] = email;
            TempData["password"] = password;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Freelancer freelancer)
        {

            // remove the properties and assign value
            ModelState.Remove("ProfileImageName");
            ModelState.Remove("FreelancerId");

            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(freelancer.ProfileImage.FileName);
            string extension = Path.GetExtension(freelancer.ProfileImage.FileName);
            fileName += DateTime.Now.ToString("yymmssffff") + extension;
            freelancer.ProfileImageName = fileName;
            string path = Path.Combine(wwwRootPath, "uploads/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await freelancer.ProfileImage.CopyToAsync(fileStream);
            }

            // alpha numeric - 10 characters
            var id = Guid.NewGuid().ToString().Replace("-", String.Empty).Substring(0, 10);

            freelancer.FreelancerId = id;

            if (ModelState.IsValid)
            {
                // save image to wwwroot/images/
                _db.Freelancer.Add(freelancer);
                _db.SaveChanges();

                Globals.UserId = id;
                Globals.UserType = Globals.UserTypes.Freelancer.ToString();

                return RedirectToAction("Index", "Home", new { userId = id, userType = Globals.UserTypes.Freelancer.ToString() });

            }

            TempData["email"] = freelancer.Email;
            TempData["password"] = freelancer.Password;
            return View();
        }

        public ActionResult Edit(string? id)
        {
            if (id == null || id == "")
            {
                return NotFound();
            }

            var freelancer = _db.Freelancer.Find(id);

            if (freelancer == null)
            {
                return NotFound();
            }

            Globals.ProfileImageName = freelancer.ProfileImageName;
            Globals.Email = freelancer.Email;
            Globals.Password = freelancer.Password;

            ViewBag.UserId = freelancer?.FreelancerId;
            ViewBag.UserType = "Freelancer";
            ViewBag.UserName = freelancer?.FreelancerName;

            TempData["email"] = freelancer?.Email;
            TempData["password"] = freelancer?.Password;

            return View(freelancer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Freelancer freelancer)
        {

            ModelState.Remove("ProfileImageName");
            ModelState.Remove("Email");
            ModelState.Remove("Password");
            ModelState.Remove("FreelancerId");

            DeleteImage(Globals.ProfileImageName);

            if (freelancer == null)
            {
                return NotFound();
            }

            freelancer.ProfileImageName = await UploadImage(freelancer.ProfileImage);
            freelancer.Email = Globals.Email;
            freelancer.Password = Globals.Password;
            freelancer.FreelancerId = Globals.UserId;

            if (ModelState.IsValid)
            {
                _db.Freelancer.Update(freelancer);
                _db.SaveChanges();
                TempData["success"] = "Freelancer updated successfully";
                return RedirectToAction("Index", "Freelancer", new { id = Globals.UserId });
            }

            ViewBag.UserId = freelancer?.FreelancerId;
            ViewBag.UserType = "Freelancer";
            ViewBag.UserName = freelancer?.FreelancerName;

            TempData["email"] = freelancer?.Email;
            TempData["password"] = freelancer?.Password;

            return View(freelancer);
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

