using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BorrowController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BorrowController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var borrows = _dbContext.Borrows.ToList();
            foreach (var borrow in borrows)
            {
                borrow.UserName = _dbContext.Users.FirstOrDefault(b => b.Id == borrow.UserId).UserName;
            }
            return View(borrows);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Users = _dbContext.Users.ToList().Select(u => new SelectListItem() { Text = u.UserName, Value = u.Id.ToString() });
            return View();
        }

        [HttpPost]
        public IActionResult Create(Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Borrows.Add(borrow);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Edit), new { id = borrow.Id });
            }

            return View(borrow);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var borrow = _dbContext.Borrows.Find(id);
            var bookBorrow = GetBorrowedBooks(id);

            ViewBag.Books = bookBorrow;
            ViewBag.Users = _dbContext.Users.ToList().Select(u => new SelectListItem() { Text = u.UserName, Value = u.Id.ToString() });
            return View(borrow);
        }

        [HttpPost]
        public IActionResult Edit(Borrow borrow)
        {
            ViewBag.Users = _dbContext.Users.ToList().Select(u => new SelectListItem() { Text = u.UserName, Value = u.Id.ToString() });
            if (ModelState.IsValid)
            {
                _dbContext.Borrows.Update(borrow);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(borrow);
        }

        [HttpGet]
        public IActionResult EditBooks(int id)
        {
            var borrow = _dbContext.Borrows.Find(id);
            var bookBorrow = GetBorrowedBooks(id);

            ViewBag.Books = bookBorrow;
            ViewBag.BorrowId = borrow.Id;

            return View();
        }

        [HttpPost]
        public IActionResult EditBooks(string searchText, string searchFor, int id)
        {
            if (searchFor == "Book" && searchText != "" && searchText != null)
            {
                var books = _dbContext.Books.Where(b => b.Title.Contains(searchText.Trim())).OrderBy(b => b.Title).ToList();
                FillBooksDetails(books);

                var borrow = _dbContext.Borrows.FirstOrDefault(b => b.Id == id);
                var bookBorrow = GetBorrowedBooks(id);

                ViewBag.Books = bookBorrow;
                ViewBag.BorrowId = borrow.Id;
                

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
                FillBooksDetails(books);

                var borrow = _dbContext.Borrows.Find(id);
                var bookBorrow = GetBorrowedBooks(id);

                ViewBag.Books = bookBorrow;
                ViewBag.BorrowId = borrow.Id;
                

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
                FillBooksDetails(books);

                var borrow = _dbContext.Borrows.Find(id);
                var bookBorrow = GetBorrowedBooks(id);

                ViewBag.Books = bookBorrow;
                ViewBag.BorrowId = borrow.Id;
                

                return View(books);
            }
            else
            {
                var borrow = _dbContext.Borrows.Find(id);
                var bookBorrow = GetBorrowedBooks(id);

                ViewBag.Books = bookBorrow;
                ViewBag.BorrowId = borrow.Id;

                return View();
            }
        }

        public IActionResult AddToBorrowList(int bookId, int id)
        {
            BookBorrow bb = new BookBorrow() {  BookId = bookId, BorrowId = id };
            _dbContext.BookBorrow.Add(bb);
            _dbContext.SaveChanges();

            var bookBorrow = GetBorrowedBooks(id);

            ViewBag.Books = bookBorrow;
            ViewBag.BorrowId = id;

            return RedirectToAction(nameof(EditBooks), new { id = id });
        }

        public IActionResult RemoveFromBorrowList(int bookId, int id)
        {
            var bb = _dbContext.BookBorrow.FirstOrDefault(bb => bb.BookId == bookId && bb.BorrowId == id);
            _dbContext.BookBorrow.Remove(bb);
            _dbContext.SaveChanges();

            var bookBorrow = GetBorrowedBooks(id);

            ViewBag.Books = bookBorrow;
            ViewBag.BorrowId = id;

            return RedirectToAction(nameof(EditBooks), new { id = id });
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

        private List<BookBorrow> GetBorrowedBooks(int id)
        {
            return _dbContext.BookBorrow.Where(bb => bb.BorrowId == id).Select(bb => new BookBorrow()
            {
                Id = bb.Id,
                BookId = bb.BookId,
                BorrowId = bb.BorrowId,
                BookTitle = _dbContext.Books.FirstOrDefault(b => b.Id == bb.BookId).Title
            }).ToList();
        }

    }
}
