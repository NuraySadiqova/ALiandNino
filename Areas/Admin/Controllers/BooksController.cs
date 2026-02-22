using AliAndNinoClone.DAL;
using AliAndNinoClone.Models;
using AliAndNinoClone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliAndNinoClone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Books


        public async Task<IActionResult> Index()
        {
            // Kitabları gətirəndə Category-ni də mütləq Include et!
            var books = await _context.Books
                                      .Include(b => b.Category)
                                      .ToListAsync();

            return View(books);
        }
        // GET: Admin/Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateVM bookVM)
        {
            // 1. Validasiya yoxlanışı (Bura boş gəlsə proqram artıq partlamır)
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", bookVM.CategoryId);
                return View(bookVM);
            }

            // 2. Şəkil yükləmə məntiqi
            string fileName = Guid.NewGuid().ToString() + "_" + bookVM.Photo.FileName;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assets/Images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await bookVM.Photo.CopyToAsync(stream);
            }

            // 3. VM-dən əsl Modelə Mapping (Köçürmə)
            Book newBook = new Book
            {
                Title = bookVM.Title,
                Author = bookVM.Author,
                Price = bookVM.Price,
                CategoryId = bookVM.CategoryId,
                ImageUrl = fileName // Faylın adını bazaya yazırıq
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Books/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            BookEditVM editVM = new BookEditVM
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                StockCount = book.StockCount,
                CategoryId = book.CategoryId,
                ExistingImageUrl = book.ImageUrl // Köhnə şəkli yadda saxlayırıq
            };

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(editVM);
        }

        // POST: Admin/Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookEditVM editVM)
        {
            if (id != editVM.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null) return NotFound();

                // Şəkil yükləmə yoxlanışı
                if (editVM.Photo != null)
                {
                    // Yeni şəkil gəlibsə, köhnəsini silirik (isteğe bağlı) və yenisini yazırıq
                    string fileName = Guid.NewGuid().ToString() + "_" + editVM.Photo.FileName;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assets/Images", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await editVM.Photo.CopyToAsync(stream);
                    }
                    book.ImageUrl = fileName; // Yeni şəklin adı
                }
                // Əgər Photo null-dursa, book.ImageUrl-ə toxunmuruq (köhnə qalır)

                book.Title = editVM.Title;
                book.Author = editVM.Author;
                book.Price = editVM.Price;
                book.StockCount = editVM.StockCount;
                book.CategoryId = editVM.CategoryId;

                _context.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", editVM.CategoryId);
            return View(editVM);
        }
        // GET: Admin/Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Admin/Books/Delete/5
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
