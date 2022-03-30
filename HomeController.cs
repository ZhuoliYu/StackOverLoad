using FinalProgram_9.Data;
using FinalProgram_9.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace FinalProgram_9.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }



        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]

        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string newRole)
        {
            if (newRole == null)
            {
                return BadRequest();
            }
            await _roleManager.CreateAsync(new IdentityRole(newRole));
            _db.SaveChanges();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SetUserRole()
        {
            SelectList userSelectList = new SelectList(_db.Users, "UserName", "UserName");
            SelectList roleSelectList = new SelectList(_db.Roles, "Name", "Name");
            ViewBag.userSelectList = userSelectList;
            ViewBag.roleSelectList = roleSelectList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SetUserRole(string UserName, string RoleName)
        {
            if (UserName == null || RoleName == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(UserName);
            if (await _roleManager.RoleExistsAsync(RoleName))
            {
                if (!await _userManager.IsInRoleAsync(user, RoleName))
                {
                    await _userManager.AddToRoleAsync(user, RoleName);
                }
                //_db.SaveChanges();
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("SetUserRole");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}