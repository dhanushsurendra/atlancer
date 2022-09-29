using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Atlancer.Controllers
{
    public class GigsController : Controller
    {

        public readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public GigsController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: GigsController
        public ActionResult Create(string? id)
        {
            ViewBag.UserId = id;
            ViewBag.UserType = "Freelancer";

            var freelancer = _db.Freelancer.Find(id);
            ViewBag.UserName = freelancer?.FreelancerName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Gigs gig) 
        {
            // remove the properties and assign value
            ModelState.Remove("GigImageName1");
            ModelState.Remove("GigImageName2");
            ModelState.Remove("GigImageName3");
            ModelState.Remove("FreelancerId");
            ModelState.Remove("Freelancer");
            ModelState.Remove("GigId");

            // alpha numeric - 10 characters
            var id = Guid.NewGuid().ToString().Replace("-", String.Empty).Substring(0, 10);

            // assign the values to the properties of the class
            // save image to wwwroot/uploads/
            gig.GigImageName1 = await UploadImage(gig.GigImage1);
            gig.GigImageName2 = await UploadImage(gig.GigImage2);
            gig.GigImageName3 = await UploadImage(gig.GigImage3);

            // fetch freelancer data
            var freelancer = _db.Freelancer.Find(Globals.UserId);
     
            if (freelancer != null)
            {
                gig.Freelancer = freelancer;
            }

            gig.FreelancerId = Globals.UserId;
           
            gig.GigId = id;

            if (ModelState.IsValid)
            {
                _db.Gig.Add(gig);
                _db.SaveChanges();

                return RedirectToAction("Index", "Freelancer", new { id = Globals.UserId });

            }

            return View();
        }

        // GET: GigsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GigsController/Edit/5
        public ActionResult Edit(string? id)
        {
            if (id == null || id == "")
            {
                return NotFound();
            }

            var gig = _db.Gig.Find(id);

            if (gig == null)
            {
                return NotFound();
            }

            Globals.GigId = id;
            Globals.GigImageName1 = gig.GigImageName1;
            Globals.GigImageName2 = gig.GigImageName2;
            Globals.GigImageName3 = gig.GigImageName3;
            Globals.GigImage1 = gig.GigImage1;
            Globals.GigImage2 = gig.GigImage2;
            Globals.GigImage3 = gig.GigImage3;
            

            var freelancer = _db.Freelancer.Find(gig.FreelancerId);

            ViewBag.UserId = freelancer?.FreelancerId;
            ViewBag.UserType = "Freelancer";
            ViewBag.UserName = freelancer?.FreelancerName;

            return View(gig);
        }

        // POST: GigsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Gigs gig)
        {

            ModelState.Remove("FreelancerId");
            ModelState.Remove("GigId");
            ModelState.Remove("GigImageName1");
            ModelState.Remove("GigImageName2");
            ModelState.Remove("GigImageName3");
            ModelState.Remove("Freelancer");

            DeleteImage(Globals.GigImageName1);
            DeleteImage(Globals.GigImageName2);
            DeleteImage(Globals.GigImageName3);
            
            gig.GigImageName1 = await UploadImage(gig.GigImage1);
            gig.GigImageName2 = await UploadImage(gig.GigImage2);
            gig.GigImageName3 = await UploadImage(gig.GigImage3);

            gig.FreelancerId = Globals.UserId;

            var freelancer = _db.Freelancer.Find(Globals.UserId);

            if (freelancer != null)
            {
                gig.Freelancer = freelancer;
            }

            gig.GigId = Globals.GigId;

            if (ModelState.IsValid)
            {
                _db.Gig.Update(gig);
                _db.SaveChanges();
                TempData["success"] = "Gig updated successfully";
                return RedirectToAction("Index", "Freelancer", new { id = Globals.UserId });
            }
            

            return View(gig);
        }

        // GET: GigsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GigsController/Delete/5
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

        // return the file name of the uploaded image
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
