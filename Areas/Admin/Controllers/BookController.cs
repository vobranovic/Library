using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BookController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index(string searchText, string searchFor)
        {
            if (searchFor == "Book" && searchText != "" && searchText != null)
                {
                    var books = _dbContext.Books.Where(b => b.Title.Contains(searchText.Trim())).OrderBy(b => b.Title).ToList();
                    CountAvailableBooks(books);
                    FillBooksDetails(books);
                
                    return View(books);
                }
            else if (searchFor == "Author" && searchText != "" && searchText != null)
            {
                var authors = _dbContext.Authors.Where(a => a.FirstName.Contains(searchText.Trim()) || a.LastName.Contains(searchText.Trim())).ToList();
                List<BookAuthor> bookAuthors = new List<BookAuthor>();
                foreach (var author in authors)
                {                  
                    bookAuthors.AddRange(_dbContext.BookAuthor.Where(ba => ba.AuthorId == author.Id).ToList());
                }

                List<Book> books = new List<Book>();
                foreach (var item in bookAuthors)
                {
                    books.AddRange(_dbContext.Books.Where(b => b.Id == item.BookId).ToList());
                }
                CountAvailableBooks(books);
                FillBooksDetails(books);

                return View(books);
            }

            else if (searchFor == "Publisher" && searchText != "" && searchText != null)
            {
                var publishers = _dbContext.Publishers.Where(p => p.Name.Contains(searchText.Trim())).ToList();
                List<Book> books = new List<Book>();
                foreach (var publisher in publishers)
                {
                    books.AddRange(_dbContext.Books.Where(b => b.PublisherId == publisher.Id).ToList());
                }
                CountAvailableBooks(books);
                FillBooksDetails(books);
                return View(books);
            }

            else if (searchFor == "All")
            {
                var books = _dbContext.Books.OrderBy(b => b.Title).ToList();
                CountAvailableBooks(books);
                FillBooksDetails(books);
                return View(books);
            }


            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var book = _dbContext.Books.Find(id);
            ViewBag.Publishers = _dbContext.Publishers.ToList().Select(p => new SelectListItem() { Text = p.Name, Value = p.Id.ToString() });

            var bookBorrows = _dbContext.BookBorrow.Where(bb => bb.BookId == book.Id);
            List<Borrow> borrows = new List<Borrow>();

            foreach (var bookBorrow in bookBorrows)
            {
                borrows.Add(_dbContext.Borrows.FirstOrDefault(b => b.Id == bookBorrow.BorrowId));
            }

            foreach (var borrow in borrows)
            {
                borrow.UserName = _dbContext.Users.FirstOrDefault(u => u.Id == borrow.UserId).UserName;
            }
            
            ViewBag.Borrows = borrows;


            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Publishers = _dbContext.Publishers.ToList().Select(p => new SelectListItem() { Text = p.Name, Value = p.Id.ToString() });

            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Books.Add(book);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Edit), new { id = book.Id});
            }

            return View(book);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _dbContext.Books.Find(id);
            ViewBag.Publishers = _dbContext.Publishers.ToList().Select(p => new SelectListItem() { Text = p.Name, Value = p.Id.ToString() });
            
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Books.Update(book);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = _dbContext.Books.Find(id);
            FillBooksDetails(book);
            return View(book);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            var book = _dbContext.Books.Find(id);
            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        private void CountAvailableBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                book.Available = book.Stock - _dbContext.BookBorrow.Count(b => b.BookId == book.Id);
            }
        }

        private void FillBooksDetails(Book book)
        {
            book.BookAuthor = _dbContext.BookAuthor.Where(ba => ba.BookId == book.Id).Select(ba => new BookAuthor
            {
                AuthorName = _dbContext.Authors.FirstOrDefault(a => a.Id == ba.AuthorId).FirstName + " " + _dbContext.Authors.FirstOrDefault(a => a.Id == ba.AuthorId).LastName
            }).ToList();
            book.Publisher = _dbContext.Publishers.FirstOrDefault(p => p.Id == book.PublisherId).Name;
        }

        private void FillBooksDetails(List<Book> books)
        {
            foreach (var book in books)
            {
                book.BookAuthor = _dbContext.BookAuthor.Where(ba => ba.BookId == book.Id).Select(ba => new BookAuthor
                {
                    AuthorName = _dbContext.Authors.FirstOrDefault(a => a.Id == ba.AuthorId).FirstName + " " + _dbContext.Authors.FirstOrDefault(a => a.Id == ba.AuthorId).LastName
                }).ToList();
                book.Publisher = _dbContext.Publishers.FirstOrDefault(p => p.Id == book.PublisherId).Name;
            }
        }
    }
}
