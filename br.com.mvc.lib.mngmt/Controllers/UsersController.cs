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
    public class UsersController : Controller
    {
        private readonly MNGMTContext _context;

        public UsersController(MNGMTContext context)
        {
            _context = context;
        }

        // GET: Users
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Select(x => new UserViewModel().ToViewModel(x)).ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(new UserViewModel().ToViewModel(user));
        }

        // GET: Users/Create
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            ViewBag.Roles = new MultiSelectList(
                Enum.GetNames(typeof(Roles))
                .Select(x => 
                    new
                    {
                        Id = x, 
                        Desc = x
                    }), "Id", "Desc");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([Bind("Name,Username,Password,Id")] UserViewModel user, string[] roles)
        {
            if (ModelState.IsValid)
            {
                if (new bizrules.User().UserUsernameMustBeUnique(user.Id, user.Username) == null)
                {
                    user.Id = Guid.NewGuid();
                    user.Roles = roles;
                    _context.Add(user.ToModel());
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Roles = new MultiSelectList(
                Enum.GetNames(typeof(Roles))
                    .Select(x =>
                        new
                        {
                            Id = x,
                            Desc = x
                        }), "Id", "Desc");
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var u = new UserViewModel().ToViewModel(user);
            u.Password = "";
            ViewBag.Roles = new MultiSelectList(
                Enum.GetNames(typeof(Roles))
                    .Select(x =>
                        new
                        {
                            Id = x,
                            Desc = x
                        }), "Id", "Desc", u.Roles);
            return View(u);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Username,Password,Id")] UserViewModel user, string[] roles)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(new bizrules.User().UserUsernameMustBeUnique(id, user.Username) != null)
                    {
                        user.Roles = roles;
                        _context.Update(user.ToModel());
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }

            ViewBag.Roles = new MultiSelectList(
                Enum.GetNames(typeof(Roles))
                    .Select(x =>
                        new
                        {
                            Id = x,
                            Desc = x
                        }), "Id", "Desc");
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(new UserViewModel().ToViewModel(user));
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
