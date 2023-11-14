using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Liga.Models;
using LigaWspinaczkowa.Data;

namespace LigaWspinaczkowa.Controllers
{
    public class UserStageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserStageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserStage
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserStage.Include(u => u.Stage).Include(u => u.UserStageUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserStage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserStage == null)
            {
                return NotFound();
            }

            var userStage = await _context.UserStage
                .Include(u => u.Stage)
                .Include(u => u.UserStageUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userStage == null)
            {
                return NotFound();
            }

            return View(userStage);
        }

        // GET: UserStage/Create
        public IActionResult Create()
        {
            ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id");
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserStage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StageId,Date,Route1Points,Route2Points,RouteLead3Points,IsAccepted,UserStageUserId")] UserStage userStage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userStage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id", userStage.StageId);
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            return View(userStage);
        }

        // GET: UserStage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserStage == null)
            {
                return NotFound();
            }

            var userStage = await _context.UserStage.FindAsync(id);
            if (userStage == null)
            {
                return NotFound();
            }
            ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id", userStage.StageId);
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            return View(userStage);
        }

        // POST: UserStage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StageId,Date,Route1Points,Route2Points,RouteLead3Points,IsAccepted,UserStageUserId")] UserStage userStage)
        {
            if (id != userStage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userStage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserStageExists(userStage.Id))
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
            ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id", userStage.StageId);
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            return View(userStage);
        }

        // GET: UserStage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserStage == null)
            {
                return NotFound();
            }

            var userStage = await _context.UserStage
                .Include(u => u.Stage)
                .Include(u => u.UserStageUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userStage == null)
            {
                return NotFound();
            }

            return View(userStage);
        }

        // POST: UserStage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserStage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserStage'  is null.");
            }
            var userStage = await _context.UserStage.FindAsync(id);
            if (userStage != null)
            {
                _context.UserStage.Remove(userStage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserStageExists(int id)
        {
          return (_context.UserStage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
