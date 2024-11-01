using BlogSite.Context;
using BlogSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.IO; 
using System.Threading.Tasks;

namespace BlogSite.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _context.Blogs.Include(b => b.User).ToListAsync();
            return View(blogs);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // Resmi kaydet
                if (imageFile != null && imageFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    blog.ImageUrl = $"/images/{imageFile.FileName}"; // Resim URL'sini ayarla
                }

                _context.Blogs.Add(blog); // Blog gönderisini ekle
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                return RedirectToAction(nameof(Index)); // Ana sayfaya yönlendir
            }
            return View(blog); // Hata varsa formu tekrar göster
        }

        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Blog model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var blog = await _context.Blogs.FindAsync(model.Id);
                if (blog == null)
                {
                    return NotFound();
                }

                // Mevcut verileri güncelle
                blog.Title = model.Title;
                blog.Content = model.Content;
                blog.UpdatedAt = DateTime.Now;

                if (imageFile != null)
                {
                    var imagePath = Path.Combine("wwwroot/images", imageFile.FileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(imagePath)); // Dosya yolunu oluşturur

                    using var stream = new FileStream(imagePath, FileMode.Create);
                    await imageFile.CopyToAsync(stream);
                    blog.ImageUrl = "/images/" + imageFile.FileName;
                }

                _context.Blogs.Update(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
