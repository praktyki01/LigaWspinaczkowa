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
        private readonly UserManager<ApplicationUser> _userManager;

        public UserStageController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexAdmin()
        {
            var applicationDbContext = _context.UserStage.OrderByDescending(u => u.Stage.DataFrom).
                Include(u => u.Stage).Include(u => u.UserStageUser);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> RankingStages()
        {
            var applicationDbContext = _context.UserStage.OrderByDescending(u => u.Stage.DataTo).
                ThenByDescending(u => (u.Route1Points + u.Route2Points + u.RouteLead3Points)).Include(u => u.Stage).Include(u => u.UserStageUser);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Ranking()
        {
            var rankingList = _context.UserStage.
                OrderByDescending(u => u.Stage.DataTo).
                ThenByDescending(u => (u.Route1Points + u.Route2Points + u.RouteLead3Points)).
                Include(u => u.Stage).Include(u => u.UserStageUser);
            return View(await rankingList.ToListAsync());
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id");
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToString("dd/MM/yyyy") + " - " + a.DataTo.ToString("dd/MM/yyyy"),
                    Value = a.Id.ToString()
                }).ToList();
            //ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["UserStageUserId"] = _context.Users
               .Select(a => new SelectListItem()
               {
                   Text = a.Surname + " " + a.Name,
                   Value = a.Id.ToString()
               }).ToList();
            return View();
        }

        // POST: UserStage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,StageId,DateRoute1,Route1Points,IsAcceptedRoute1,DateRoute2,Route2Points,IsAcceptedRoute2,DateRoute3,RouteLead3Points,IsAcceptedRoute3,UserStageUserId")] UserStage userStage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userStage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAdmin));
            }
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToString("dd-MM-yyyy") + " - " + a.DataTo.ToString("dd/MM/yyyy"),
                    Value = a.Id.ToString()
                }).ToList();
            //ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            ViewData["UserStageUserId"] = _context.Users
               .Select(a => new SelectListItem()
               {
                   Text = a.Surname + " " + a.Name,
                   Value = a.Id.ToString()
               }).ToList();
            return View(userStage);
        }

        [Authorize]
        public async Task<IActionResult> CreateUser()
        {
            //ViewData["StageId"] = new SelectList(_context.Stage.OrderByDescending(a => a.DataTo), "Id", "DataTo");
            DateTime currentDate = DateTime.Now;
            var pointsList = _context.Stage.Where(s => s.DataTo >= currentDate).FirstOrDefault();
            if (pointsList != null)
                ViewBag.MaxPointsList = pointsList.Name;
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
            {
                Text = a.DataFrom.ToString("dd-MM-yyyy") + " - " + a.DataTo.ToString("dd-MM-yyyy"),
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

                //chcek max points
                var maxPointsListS = _context.Stage.Where(us => us.Id == userStage.StageId).FirstOrDefault();
                if (maxPointsListS != null && maxPointsListS.Name != null)
                {
                    List<string> maxListPointList = maxPointsListS.Name.Split(';').ToList();
                    List<float> maxPointsFloat = new List<float>();
                    if (maxListPointList.Count() >= 3)
                    {
                        foreach (var point in maxListPointList)
                        {
                            float pointValue;
                            float.TryParse(point, out pointValue);
                            maxPointsFloat.Add(pointValue);
                        }
                        if (userStage.Route1Points > maxPointsFloat[0] || userStage.Route2Points > maxPointsFloat[1]
                            || userStage.RouteLead3Points > maxPointsFloat[2])
                        {
                            return RedirectToAction(nameof(IndexUser));
                        }
                    }
                }
                //chcek max points


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexUser));
            }
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "DataTo", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo).Select(a => new SelectListItem() { 
                Text = a.DataFrom.ToString("dd/MM/yyyy") + " - " + a.DataTo.ToString("dd/MM/yyyy"), Value = a.Id.ToString() }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            DateTime currentDate = DateTime.Now;
            var pointsList = _context.Stage.Where(s => s.DataTo >= currentDate).FirstOrDefault();
            if (pointsList != null)
                ViewBag.MaxPointsList = pointsList.Name;
            return View(userStage);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptUserStage(int? id)
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
            userStage.IsAcceptedRoute1 = true;
            userStage.IsAcceptedRoute2 = true;
            userStage.IsAcceptedRoute3 = true;
            _context.Update(userStage);
            _context.SaveChanges();
            return RedirectToAction(nameof(IndexAdmin)); 
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
                    Text = a.DataFrom.ToString("dd/MM/yyyy") + " - " + a.DataTo.ToString("dd/MM/yyyy"),
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
                    Text = a.DataFrom.ToString("dd/MM/yyyy") + " - " + a.DataTo.ToString("dd/MM/yyyy"),
                    Value = a.Id.ToString()
                }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            return View(userStage);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAdmin(int? id)
        {
            if (id == null || _context.UserStage == null)
            {
                return NotFound();
            }

            var userStage = await _context.UserStage.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (userStage == null)
            {
                return NotFound();
            }
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "DataTo", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToString("dd/MM/yyyy") + " - " + a.DataTo.ToString("dd/MM/yyyy"),
                    Value = a.Id.ToString()
                }).ToList();
            //ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            ViewData["UserStageUserId"] = _context.Users
               .Select(a => new SelectListItem()
               {
                   Text = a.Surname + " " + a.Name,
                   Value = a.Id.ToString()
               }).ToList();
            //ViewBag.user = userStage.UserStageUser.Email;
            return View(userStage);
        }

        // POST: UserStage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAdmin(int id, [Bind("Id,StageId,DateRoute1,Route1Points,IsAcceptedRoute1,DateRoute2,Route2Points,IsAcceptedRoute2,DateRoute3,RouteLead3Points,IsAcceptedRoute3,UserStageUserId")] UserStage userStage)
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
                return RedirectToAction(nameof(IndexAdmin));
            }
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "Id", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToString("dd/MM/yyyy") + " - " + a.DataTo.ToString("dd/MM/yyyy"),
                    Value = a.Id.ToString()
                }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            //ViewBag.user = userStage.UserStageUser.Email;
            return View(userStage);
        }

        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null || _context.UserStage == null)
            {
                return NotFound();
            }

            //var userStage = await _context.UserStage.FindAsync(id);
            var userStage = _context.UserStage.Include(n=>n.Stage).Where(n=>n.Id== id).FirstOrDefault();
            if (userStage == null)
            {
                return NotFound();
            }
            //ViewData["StageId"] = new SelectList(_context.Stage, "Id", "DataTo", userStage.StageId);
            ViewData["StageId"] = _context.Stage.OrderByDescending(b => b.DataTo)
                .Select(a => new SelectListItem()
                {
                    Text = a.DataFrom.ToString("dd/MM/yyyy") + " - " + a.DataTo.ToString("dd/MM/yyyy"),
                    Value = a.Id.ToString()
                }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            DateTime currentDate = DateTime.Now;
            var pointsList = _context.Stage.Where(s => s.DataTo >= currentDate).FirstOrDefault();
            if (pointsList != null)
                ViewBag.MaxPointsList = pointsList.Name;
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
                    //chcek max points
                    var maxPointsListS = _context.UserStage.Where(us=>us.StageId==userStage.StageId).Select(s=>s.Stage).FirstOrDefault();
                    if(maxPointsListS!=null && maxPointsListS.Name!=null)
                    {
                        List<string> maxListPointList = maxPointsListS.Name.Split(';').ToList();
                        List<float> maxPointsFloat = new List<float>();
                        if(maxListPointList.Count()>=3)
                        {
                            foreach(var point in maxListPointList)
                            {
                                float pointValue;
                                float.TryParse(point, out pointValue);
                                maxPointsFloat.Add(pointValue);
                            }
                            if (userStage.Route1Points > maxPointsFloat[0] || userStage.Route2Points > maxPointsFloat[1]
                                || userStage.RouteLead3Points > maxPointsFloat[2])
                            {
                                return RedirectToAction(nameof(IndexUser));
                            }
                        }
                    }
                    //chcek max points


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
                    Text = a.DataFrom.ToString("dd/MM/yyyy") + " - " + a.DataTo.ToString("dd/MM/yyyy"),
                    Value = a.Id.ToString()
                }).ToList();
            ViewData["UserStageUserId"] = new SelectList(_context.Users, "Id", "Id", userStage.UserStageUserId);
            DateTime currentDate = DateTime.Now;
            var pointsList = _context.Stage.Where(s => s.DataTo >= currentDate).FirstOrDefault();
            if (pointsList != null)
                ViewBag.MaxPointsList = pointsList.Name;
            return View(userStage);
        }

        // GET: UserStage/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
            return RedirectToAction(nameof(IndexAdmin));
        }

        public async Task<IActionResult> DeleteUser(int? id)
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
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedUser(int id)
        {
            if (_context.UserStage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserStage'  is null.");
            }
            var userStage = await _context.UserStage.FindAsync(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (userStage != null && user.Id == userStage.UserStageUserId &&
                !(userStage.IsAcceptedRoute1||userStage.IsAcceptedRoute2||userStage.IsAcceptedRoute3))
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
