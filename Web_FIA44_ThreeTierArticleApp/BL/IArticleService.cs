using Web_FIA44_ThreeTierArticleApp.Models;

namespace Web_FIA44_ThreeTierArticleApp.BL
{
	// Interface für die Artikelverwaltung (diese ist für den Businesslayer gedacht)also für die Geschäftslogik
	public interface IArticleService
	{
		void AddArticle(Article article);
		bool UpdateArticle(Article article);
		void DeleteArticle(int AId);
		Article GetArticleById(int AId);
		List<Article> GetAllArticles(int pageNumber, int pageSize);
		int GetArticleCount();
		decimal GetStockValue();
		public (int availableInStock, int notAvailableNotinStock, int inStockNotAvailable, int availableNotInStock) GetStockStatus();

		public (string Name, decimal StockValue) GetArticleMaxStockValue();
		public (string Name, decimal Price) GetArticleMinPrice();

		public string IsArticleAvailableAndInStock(int AId);

	
	}
}
