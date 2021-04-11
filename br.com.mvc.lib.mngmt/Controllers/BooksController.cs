using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using br.com.mvc.lib.mngmt.model;
using br.com.mvc.lib.mngmt.repository;
using br.com.mvc.lib.mngmt.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace br.com.mvc.lib.mngmt.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly MNGMTContext _context;

        public BooksController(MNGMTContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var b = await _context.Books.Where(x => x.Status == BookStatus.AVALIABLE).ToListAsync();
            var bm = b.Select(x => new BookViewModel().ToViewModel(x, 1));
            return View(bm);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(new BookViewModel().ToViewModel(book, 1));
        }

        // GET: Books/Create
        [Authorize(Roles = "ADMIN,USER")]
        public IActionResult Create()
        {
            ViewBag.Authors = new MultiSelectList(_context.Authors.Select(x => new AuthorViewModel().ToViewModel(x, 1)).ToList(), "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> Create([Bind("Title,Id,Quantity")] BookViewModel book, Guid[] authors)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < book.Quantity; i++)
                {
                    var mBook = book.ToModel();
                    mBook.Id = Guid.NewGuid();

                    foreach (var a in authors)
                    {
                        mBook.Author.Add(await _context.Authors.FirstOrDefaultAsync(x => x.Id == a));
                    }

                    _context.Add(mBook);
                    await _context.SaveChangesAsync();
                }


                return RedirectToAction(nameof(Index));
            }
            ViewBag.Authors = new MultiSelectList(_context.Authors.Select(x => new AuthorViewModel().ToViewModel(x, 1)).ToList(), "Id", "Name");
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> Edit(Guid? id)
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
            ViewBag.Authors = new MultiSelectList(_context.Authors.Select(x => new AuthorViewModel().ToViewModel(x, 1)).ToList(), "Id", "Name", book.Author.Select(x => x.Id));
            return View(new BookViewModel().ToViewModel(book, 1));
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Id,Quantity")] BookViewModel book, Guid[] authors)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var b = await _context.Books.FirstOrDefaultAsync(x => x.Id == book.Id);
                    b.Author.Clear();
                    _context.Update(b);
                    await _context.SaveChangesAsync();
                    _context.Entry(b).State = EntityState.Detached;

                    var mBook = book.ToModel();
                    foreach (var a in authors)
                    {
                        mBook.Author.Add(await _context.Authors.FirstOrDefaultAsync(x => x.Id == a));
                    }

                    _context.Update(mBook);
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
            ViewBag.Authors = new MultiSelectList(_context.Authors.Select(x => new AuthorViewModel().ToViewModel(x, 1)).ToList(), "Id", "Name", authors);
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(new BookViewModel().ToViewModel(book, 1));
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Borrow(Guid id)
        {
            var u = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == u);
            var book = await _context.Books.FindAsync(id);
            
            book.Status = BookStatus.BORROWED;

            _context.Historics.Add(new Historic()
            {
                User = user,
                Book = book,
                BorrowDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(14)
            });
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Devolution()
        {
            var u = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;

            var h = await _context.Historics.Where(x => x.User.Username == u && x.ReturnedDate == null).OrderByDescending(x => x.BorrowDate).ToListAsync();
            
            return View(h.Select(x => new HistoricViewModel().ToViewModel(x)));
        }

        public async Task<IActionResult> DevolutionRequest(Guid id)
        {
            var h = await _context.Historics.FindAsync(id);
            h.ReturnedDate = DateTime.Now;
            h.Book.Status = BookStatus.AVALIABLE;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Devolution)); 
        }

        public async Task<IActionResult> Historic()
        {
            var u = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;

            var h = await _context.Historics.Where(x => x.User.Username == u).OrderByDescending(x => x.BorrowDate).ToListAsync();

            return View(h.Select(x => new HistoricViewModel().ToViewModel(x)));
        }

        [ActionName("HistoricRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HistoricRequest(Guid id)
        {
            var h = await _context.Historics.FindAsync(id);
            h.ReturnedDate = DateTime.Now;
            h.Book.Status = BookStatus.AVALIABLE;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Historic));
        }
        

        private bool BookExists(Guid id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
