using ArticlProject.Data;
using ArticlProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace ArticlProject.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ArticlesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Articles in index main
        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles.ToListAsync();
            return View(articles);
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null) return NotFound();

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Article article)
        {
            // 1. جلب ID المستخدم المسجل دخول
            var loginUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(loginUserId))
            {
                ModelState.AddModelError("", "يجب تسجيل الدخول أولاً");
                return View(article);
            }

            // 2. البحث عن المؤلف المرتبط بالمستخدم
            var currentAuthor = await _context.Authors
                .FirstOrDefaultAsync(a => a.UserId == loginUserId);

            if (currentAuthor == null)
            {
                ModelState.AddModelError("", "حسابك غير مسجل كملف شخصي لمؤلف. يرجى مراجعة الإدارة.");
                return View(article);
            }

            // 3. تعيين المفاتيح الخارجية
            article.AuthorId = currentAuthor.AuthorId;
            article.UserId = loginUserId; // ✅ الآن ستعمل لأن UserId أصبحت nullable

            // 4. معالجة الصورة
            if (article.ImageFile != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(article.ImageFile.FileName);
                string path = Path.Combine(wwwRootPath + "/images/Articles", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await article.ImageFile.CopyToAsync(fileStream);
                }
                article.Image = fileName;
            }

            // 5. الحفظ
            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }       

        
        // GET: Articles/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            return View(article);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article)
        {
            if (id != article.ArticleId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null) return NotFound();

            return View(article);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // عرض مقالاتي
        [Authorize]
        public async Task<IActionResult> MyArticles()
        {
            var loginUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var myArticles = await _context.Articles
                .Include(a => a.Author)
                .Where(a => a.UserId == loginUserId)
                .OrderByDescending(a => a.ArticleId)
                .ToListAsync();

            return View(myArticles);
        }

        // عرض جميع الناشرين
        public async Task<IActionResult> Authors()
        {
            var authors = await _context.Authors
                .Include(a => a.Articles)
                .Where(a => a.Articles.Any()) // فقط الناشرين لديهم مقالات
                .ToListAsync();

            return View(authors);
        }
    }
}
