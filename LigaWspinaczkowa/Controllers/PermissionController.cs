using LigaWspinaczkowa.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LigaWspinaczkowa.Controllers
{
    public class PermissionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddRole()
        {
            _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            _context.SaveChanges();
            return View();
        }

        public async Task<IActionResult> AssignRole(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (!await _userManager.IsInRoleAsync(user, role)) 
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return View();
        }
    }
}
