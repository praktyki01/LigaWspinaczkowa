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
    public class StageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stage
        public async Task<IActionResult> Index()
        {
              return _context.Stage != null ? 
                          View(await _context.Stage.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Stage'  is null.");
        }

        // GET: Stage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stage == null)
            {
                return NotFound();
            }

            var stage = await _context.Stage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stage == null)
            {
                return NotFound();
            }

            return View(stage);
        }

        // GET: Stage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataFrom,DataTo,Route1Holds,Route2Holds,Route3Lead,Name,Description")] Stage stage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stage);
        }

        // GET: Stage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stage == null)
            {
                return NotFound();
            }

            var stage = await _context.Stage.FindAsync(id);
            if (stage == null)
            {
                return NotFound();
            }
            return View(stage);
        }

        // POST: Stage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataFrom,DataTo,Route1Holds,Route2Holds,Route3Lead,Name,Description")] Stage stage)
        {
            if (id != stage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StageExists(stage.Id))
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
            return View(stage);
        }

        // GET: Stage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stage == null)
            {
                return NotFound();
            }

            var stage = await _context.Stage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stage == null)
            {
                return NotFound();
            }

            return View(stage);
        }

        // POST: Stage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Stage'  is null.");
            }
            var stage = await _context.Stage.FindAsync(id);
            if (stage != null)
            {
                _context.Stage.Remove(stage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StageExists(int id)
        {
          return (_context.Stage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
