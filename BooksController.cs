﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LiteraryMaze.Data;
using Microsoft.AspNetCore.Authorization;

namespace LiteraryMaze.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string? category, string? searchString)
        {
            var allData = _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Categories)
                .Include(b => b.Genres)
                .Include(b => b.Publishers);

            if (!string.IsNullOrEmpty(searchString))
            {
                // You can search by book name or author name
                var filtered = allData.Where(b =>
                    b.Name.Contains(searchString) ||
                    b.Authors.Name.Contains(searchString)); // assuming 1 author per book

                return View(await filtered.ToListAsync());
            }

            if (category != null)
            {
                var byCategory = allData.Where(x => x.Categories.Name == category);
                return View(await byCategory.ToListAsync());
            }

            return View(await allData.ToListAsync());
        }
        //public async Task<IActionResult> Index(string? category)
        //{
        //    var allData = _context.Books.Include(b => b.Authors)
        //        .Include(b => b.Categories).Include(b => b.Genres)
        //        .Include(b => b.Publishers);
        //    if (category!= null)
        //    {
        //        var applicationDbContext = allData.Where(x => x.Categories.Name == category);
        //        return View(await applicationDbContext.ToListAsync());
        //    }
        //    return View( await allData.ToListAsync());       

        //}

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Categories)
                .Include(b => b.Genres)
                .Include(b => b.Publishers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatNumber,Name,AuthorId,PublisherId,CategoryId,GenreId,Plot,ImageURL,Price,Quantity,Year,DateRegister")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", book.CategoryId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", book.GenreId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", book.PublisherId);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CatNumber,Name,AuthorId,PublisherId,CategoryId,GenreId,Plot,ImageURL,Price,Quantity,Year,DateRegister")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", book.CategoryId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", book.GenreId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Categories)
                .Include(b => b.Genres)
                .Include(b => b.Publishers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
