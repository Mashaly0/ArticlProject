using ArticlProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
        var articles = await _context.Articles
                            .Take(5)
                            .ToListAsync();

        var model = new DashboardViewModel
        {
            CurrentUserName = User.Identity.Name,
            Articles = articles
        };

        return View(model);
    }
}
