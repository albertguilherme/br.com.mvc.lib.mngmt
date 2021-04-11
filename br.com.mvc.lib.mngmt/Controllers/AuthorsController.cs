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
    public class AuthorsController : Controller
    {
        private readonly MNGMTContext _context;

        public AuthorsController(MNGMTContext context)
        {
            _context = context;
        }

        // GET: Authors
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Authors.Select(x => new AuthorViewModel().ToViewModel(x,1)).ToListAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(new AuthorViewModel().ToViewModel(author,1));
        }

        // GET: Authors/Create
        [Authorize(Roles = "ADMIN,USER")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "ADMIN,USER")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] AuthorViewModel author)
        {
            if (ModelState.IsValid)
            {
                author.Id = Guid.NewGuid();
                _context.Add(author.ToModel());
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(new AuthorViewModel().ToViewModel(author,1));
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Id")] AuthorViewModel author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author.ToModel());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(new AuthorViewModel().ToViewModel(author,1));
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var author = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(Guid id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
