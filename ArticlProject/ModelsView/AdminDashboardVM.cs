using ArticlProject.Models;

namespace ArticlProject.ModelsView
{
    public class AdminDashboardVM
    {
        public int ArticlesCount { get; set; }
        public int AuthorsCount { get; set; }

        public List<Article> LatestArticles { get; set; }
        public List<Author> LatestAuthors { get; set; }
    }
}
