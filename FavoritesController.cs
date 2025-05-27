using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LiteraryMaze.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
namespace LiteraryMaze.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Favorites

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userFavorites = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Books)
                .ToListAsync();

            return View(userFavorites);
        }

        [HttpGet]
        public async Task<JsonResult> IsFavorite(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, isFavorite = false });
            }

            var isFav = await _context.Favorites.AnyAsync(f => f.BookId == bookId && f.UserId == userId);
            return Json(new { success = true, isFavorite = isFav });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteFavorite(int bookId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Не сте влезли в системата." });
                }

                var favorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.BookId == bookId && f.UserId == userId);

                if (favorite != null)
                {
                    _context.Favorites.Remove(favorite);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Любимото не беше намерено." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(int bookId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Не сте влезли в системата." });
                }

                if (!_context.Favorites.Any(f => f.BookId == bookId && f.UserId == userId))
                {
                    _context.Favorites.Add(new Favorite
                    {
                        BookId = bookId,
                        UserId = userId,
                        DateRegister = DateTime.Now,
                        Description = "-",
                        Quantity = 0
                    });

                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }






        // GET: Favorites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
                .Include(f => f.Books)
                .Include(f => f.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // GET: Favorites/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Favorites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,BookId,Quantity,Description,DateRegister")] Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favorite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", favorite.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", favorite.UserId);
            return View(favorite);
        }

        // GET: Favorites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", favorite.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", favorite.UserId);
            return View(favorite);
        }

        // POST: Favorites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,BookId,Quantity,Description,DateRegister")] Favorite favorite)
        {

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(favorite.Description))
                {
                    ModelState.AddModelError("Description", "Description cannot be empty.");
                }

                // If Description is empty, add an error and return to the form
                if (favorite.Quantity < 1)
                {
                    ModelState.AddModelError("Quantity", "Quantity must be at least 1.");
                }

                if (ModelState.IsValid)
                {
                    _context.Add(favorite);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", favorite.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", favorite.UserId);
            return View(favorite);

            //if (id != favorite.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(favorite);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!FavoriteExists(favorite.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", favorite.BookId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", favorite.UserId);
            //return View(favorite);
        }

        // GET: Favorites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
                .Include(f => f.Books)
                .Include(f => f.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // POST: Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteExists(int id)
        {
            return _context.Favorites.Any(e => e.Id == id);
        }
    }
}
