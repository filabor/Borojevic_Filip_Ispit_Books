using Ispit.Books.Data;
using Ispit.Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ispit.Books.Controllers
{
    [Authorize]
    public class BookAdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public BookAdminController(ApplicationDbContext context, UserManager<IdentityUser> user)
        {
            _context = context;
            _userManager = user;
        }


        // GET: BookAdminController
        public async Task<ActionResult> Index()
        {
            var books = new List<Book>();
            if (await _userManager.IsInRoleAsync(_userManager.GetUserAsync(User).Result, "admin"))
            {
                books = _context.Books.Include("Author").Include("Publisher").ToList();
            }
            else
            {
                var user_id = _userManager.GetUserId(User);
                books = _context.Books.Include("Author").Include("Publisher").Where(b => b.UserId == user_id).ToList();
            }
           
            return View(books);
        }

        // GET: BookAdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookAdminController/Create
        public ActionResult Create()
        {
            ViewBag.UserId = _userManager.GetUserId(User);
            return View();
        }

        // POST: BookAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book new_book)
        {
            try
            {
                _context.Books.Add(new_book);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            Book find_book = _context.Books.FirstOrDefault(b => b.Id == id);

            ViewBag.UserId = _userManager.GetUserId(User);

            ViewBag.Authors = _context.Authors.ToList();

            ViewBag.Publishers = _context.Publishers.ToList();

            return View(find_book);
        }

        // POST: BookAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book)
        {
            try
            {
                _context.Books.Update(book);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            if(id == 0)
            {
                return RedirectToAction("Index");
            }

            var book = _context.Books.Include("Author").Include("Publisher").SingleOrDefault(b => b.Id == id);

            if(book == null)
            {
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // POST: BookAdminController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            if(id == 0)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var find_book = _context.Books.SingleOrDefault(b => b.Id == id);

                if(find_book == null)
                {
                    return RedirectToAction("Delete");
                }

                _context.Books.Remove(find_book);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
