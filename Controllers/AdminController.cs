using ArticlProject.Data;
using ArticlProject.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Account");

        var model = new UserDashboardVM
        {
            UserName = user.UserName,
            Articles = await _context.Articles
                        .Where(a => a.UserId == user.Id)
                        .ToListAsync(),

            ArticlesCount = await _context.Articles
                        .Where(a => a.UserId == user.Id)
                        .CountAsync()
        };

        return View(model);
    }
}
