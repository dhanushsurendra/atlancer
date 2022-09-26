using Atlancer.Data;
using Atlancer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Atlancer.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProjectController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
        }
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project)
        {

            ModelState.Remove("ClientId");
            ModelState.Remove("Client");
            ModelState.Remove("ProjectId");

            var client = _db.Client.Find(Globals.UserId);

            var id = Guid.NewGuid().ToString().Replace("-", String.Empty).Substring(0, 10);

            project.ClientId = Globals.UserId;
            project.ProjectId = id;

            if (client != null)
            {
                project.Client = client;
            }

            if (ModelState.IsValid)
            {
                // save image to wwwroot/images/
                _db.Project.Add(project);
                _db.SaveChanges();

                Globals.UserType = Globals.UserTypes.Client.ToString();

                return RedirectToAction("Index", "Client", new { id = Globals.UserId });

            }

            return View();
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Project/Edit/5
        public ActionResult Edit(string? id)
        {

            if (id == "" || id == null)
            {
                return NotFound();
            }

            var project = _db.Project.Find(id);

            Globals.ProjectId = id;

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project)
        {
            ModelState.Remove("ProjectId");
            ModelState.Remove("ClientId");
            ModelState.Remove("Client");

            project.ProjectId = Globals.ProjectId;
            project.ClientId = Globals.UserId;

            var client = _db.Client.Find(Globals.UserId);

            if (client != null)
            {
                project.Client = client;
            }

            if (ModelState.IsValid)
            {
                _db.Project.Update(project);
                _db.SaveChanges();
                TempData["success"] = "Project updated successfully";
                return RedirectToAction("Index", "Client", new { id = Globals.UserId });
            }

            return View(project);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Project/Delete/5
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
