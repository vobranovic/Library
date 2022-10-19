using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Policy;
using Publisher = Library.Models.Publisher;

namespace Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PublisherController(ApplicationDbContext dbContext)
        {
             _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var publishers = _dbContext.Publishers.ToList();
            return View(publishers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Publishers.Add(publisher);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(publisher);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var publisher = _dbContext.Publishers.Find(id);
            return View(publisher);
        }

        [HttpPost]
        public IActionResult Edit(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Publishers.Update(publisher);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(publisher);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var publisher = _dbContext.Publishers.Find(id);
            return View(publisher);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            var publisher = _dbContext.Publishers.Find(id);
            _dbContext.Publishers.Remove(publisher);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
