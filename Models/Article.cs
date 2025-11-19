using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ArticlProject.Models
{
    public class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;

        // 👇 الربط مع Author
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public string UserId { get; set; } 
        public IdentityUser User { get; set; }

    }



}
