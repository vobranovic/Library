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
            try
            {
                var authors = _dbContext.Authors.ToList();
                return View(authors);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dbContext.Authors.Add(author);
                    _dbContext.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(author);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (_dbContext.Authors.Find(id) == null)
            {
                return RedirectToAction("PageNotFound", "HttpStatusCodes");
            }
            var author = _dbContext.Authors.Find(id);
            return View(author);
        }

        [HttpPost]
        public IActionResult Edit(Author author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dbContext.Authors.Update(author);
                    _dbContext.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(author);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (_dbContext.Authors.Find(id) == null)
            {
                return RedirectToAction("PageNotFound", "HttpStatusCodes");
            }
            var author = _dbContext.Authors.Find(id);
            return View(author);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            if (_dbContext.Authors.Find(id) == null)
            {
                return RedirectToAction("PageNotFound", "HttpStatusCodes");
            }
            try
            {
                var author = _dbContext.Authors.Find(id);
                _dbContext.Authors.Remove(author);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
