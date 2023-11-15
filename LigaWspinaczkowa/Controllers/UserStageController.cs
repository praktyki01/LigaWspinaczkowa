using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaWspinaczkowa.Data;
using LigaWspinaczkowa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LigaWspinaczkowa.Controllers
{
    public class UserStageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserStageController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserStage
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserStage.Include(u => u.Stage).Include(u => u.UserStageUser);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> IndexUser()
        {
            DateTime currentDate = DateTime.Now;
            var IsActiveStage = _context.Stage
                .Where(a => a.DataFrom <= currentDate && a.DataTo >= currentDate).FirstOrDefault();
            if (IsActiveStage != null)
            {
                ViewBag.IsActiveStage = true;
            } else
            {
                ViewBag.IsActiveStage = false;
            }
            
            // Zawężenie wyników wyszukiwania do aktualnie zalogowanego użytkownika
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var applicationDbContext = _context.UserStage.Include(u => u.Stage)
                .Include(u => u.UserStageUser).Where(u => u.UserStageUserId == user.Id);
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
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id");
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToShortDateString() + " - " + a.DataTo.ToShortDateString(),
                    Value = a.Id.ToString()
                }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserStage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StageId,DateRoute1,Route1Points,IsAcceptedRoute1,DateRoute2,Route2Points,IsAcceptedRoute2,DateRoute3,RouteLead3Points,IsAcceptedRoute3,UserStageUserId")] UserStage userStage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userStage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToShortDateString() + " - " + a.DataTo.ToShortDateString(),
                    Value = a.Id.ToString()
                }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            return View(userStage);
        }

        [Authorize]
        public async Task<IActionResult> CreateUser()
        {
            //ViewData["StageId"] = new SelectList(_context.Stage.OrderByDescending(a => a.DataTo), "Id", "DataTo");
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
            {
                Text = a.DataFrom.ToShortDateString() + " - " + a.DataTo.ToShortDateString(),
                Value = a.Id.ToString()
            }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserStage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateUser([Bind("Id,StageId,DateRoute1,Route1Points,IsAcceptedRoute1,DateRoute2,Route2Points,IsAcceptedRoute2,DateRoute3,RouteLead3Points,IsAcceptedRoute3,UserStageUserId")] UserStage userStage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userStage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexUser));
            }
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "DataTo", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo).Select(a => new SelectListItem() { 
                Text = a.DataFrom.ToShortDateString() + " - " + a.DataTo.ToShortDateString(), Value = a.Id.ToString() }).ToList();
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
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "DataTo", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToShortDateString() + " - " + a.DataTo.ToShortDateString(),
                    Value = a.Id.ToString()
                }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            return View(userStage);
        }

        // POST: UserStage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StageId,DateRoute1,Route1Points,IsAcceptedRoute1,DateRoute2,Route2Points,IsAcceptedRoute2,DateRoute3,RouteLead3Points,IsAcceptedRoute3,UserStageUserId")] UserStage userStage)
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
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToShortDateString() + " - " + a.DataTo.ToShortDateString(),
                    Value = a.Id.ToString()
                }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            return View(userStage);
        }

        public async Task<IActionResult> EditUser(int? id)
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
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "DataTo", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToShortDateString() + " - " + a.DataTo.ToShortDateString(),
                    Value = a.Id.ToString()
                }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            return View(userStage);
        }

        // POST: UserStage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, [Bind("Id,StageId,DateRoute1,Route1Points,IsAcceptedRoute1,DateRoute2,Route2Points,IsAcceptedRoute2,DateRoute3,RouteLead3Points,IsAcceptedRoute3,UserStageUserId")] UserStage userStage)
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
                return RedirectToAction(nameof(IndexUser));
            }
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToShortDateString() + " - " + a.DataTo.ToShortDateString(),
                    Value = a.Id.ToString()
                }).ToList();
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
            return RedirectToAction(nameof(IndexUser));
        }

        private bool UserStageExists(int id)
        {
          return (_context.UserStage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
