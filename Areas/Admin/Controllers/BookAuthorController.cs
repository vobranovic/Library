using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookAuthorController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BookAuthorController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(int id)
        {
            var bookAuthors = _dbContext.BookAuthor.Where(ba => ba.BookId == id).Select(ba => new BookAuthor 
            { 
                Id = ba.Id,
                BookId = ba.BookId,
                BookTitle = _dbContext.Books.FirstOrDefault(b => b.Id == ba.BookId).Title,
                AuthorId = ba.AuthorId,
                AuthorName = _dbContext.Authors.FirstOrDefault(a => a.Id == ba.AuthorId).FirstName + " " + _dbContext.Authors.FirstOrDefault(a => a.Id == ba.AuthorId).LastName
            }).ToList();

            ViewBag.BookId = id;
            return View(bookAuthors);
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            ViewBag.BookId = id;
            ViewBag.Authors = _dbContext.Authors.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.FirstName + " " + a.LastName
            }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Add([Bind("BookId, AuthorId")]BookAuthor bookAuthor)
        {
            if (ModelState.IsValid)
            {
                _dbContext.BookAuthor.Add(bookAuthor);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index), new { id = bookAuthor.BookId});
            }

            return View(bookAuthor);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var bookAuthor = _dbContext.BookAuthor.Find(id);
            _dbContext.BookAuthor.Remove(bookAuthor);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));

        }
    }
}
