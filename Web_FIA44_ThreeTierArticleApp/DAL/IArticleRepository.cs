using Web_FIA44_ThreeTierArticleApp.Models;

namespace Web_FIA44_ThreeTierArticleApp.DAL
{
	public interface IArticleRepository
	{
		int InsertArticle(Article article);
		bool UpdateArticle(Article article);
		bool DeleteArticle(int AId);
		Article GetArticleById(int AId);
		List<Article> GetAllArticlesByPageAndSize(int pageNumber ,int pageSize);

		List<Article> GetAllArticles();
	}
}
