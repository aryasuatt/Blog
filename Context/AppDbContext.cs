using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogSite.Models;
using Microsoft.AspNetCore.Identity;
namespace BlogSite.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Kullanıcı ekleme
            if (!context.Users.Any())
            {
                var hasher = new PasswordHasher<ApplicationUser>();
                var user = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FullName = "Admin User"
                };
                user.PasswordHash = hasher.HashPassword(user, "Admin@123");

                context.Users.Add(user);
                context.SaveChanges();

                // Kullanıcının ID'sini blog gönderileri için sakla
                var userId = user.Id;

                // Blog ekleme
                if (!context.Blogs.Any())
                {
                    context.Blogs.AddRange(new List<Blog>
            {
                new Blog
                {
                    Title = "First Blog Post",
                    Content = "This is the content of the first blog post.",
                    ImageUrl = "/images/example1.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = userId // Kullanıcının doğru ID'sini buraya ekleyin
                },
                new Blog
                {
                    Title = "Second Blog Post",
                    Content = "This is the content of the second blog post.",
                    ImageUrl = "/images/example2.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = userId // Kullanıcının doğru ID'sini buraya ekleyin
                }
            });

                    context.SaveChanges();
                }
            }
        }

    }
}

