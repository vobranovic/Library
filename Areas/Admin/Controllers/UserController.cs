using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            
            foreach (var user in users)
            {
                
                user.BooksBorrowed = 0;

                var borrows = _dbContext.Borrows.Where(b => b.UserId == user.Id);
                foreach (var borrow in borrows)
                {
                    var bbs = _dbContext.BookBorrow.Where(bb => bb.BorrowId == borrow.Id).ToList();
                    user.BooksBorrowed += bbs.Count;
                }
                
            }
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string firstName, string username, string lastName, string address, string city, string oib)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.UserName = username;
                user.Address = address;
                user.City = city;
                user.OIB = oib;

                await _userManager.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Errors(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User nout found.");
            }

            return RedirectToAction(nameof(Index));
        }


        private void Errors(IdentityResult identityResult)
        {
            foreach (IdentityError error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }


    }
}
