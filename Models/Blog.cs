using System;

namespace BlogSite.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } // Güncelleme tarihini nullable olarak ekledik
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
