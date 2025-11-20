using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticlProject.Models
{
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string About { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();
        public string? UserId { get; set; }
    }
}
