using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        
        public AuthorController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var authors = _dbContext.Authors.ToList();
            return View(authors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Authors.Add(author);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(author);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = _dbContext.Authors.Find(id);
            return View(author);
        }

        [HttpPost]
        public IActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Authors.Update(author);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(author);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var author = _dbContext.Authors.Find(id);
            return View(author);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            var author = _dbContext.Authors.Find(id);
            _dbContext.Authors.Remove(author);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
