using ArticlProject.Models;

public class UserDashboardVM
{
    public string UserName { get; set; }
    public int ArticlesCount { get; set; }

    public List<Article> Articles { get; set; }
}
