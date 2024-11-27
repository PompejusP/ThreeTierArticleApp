namespace Web_FIA44_ThreeTierArticleApp.ViewModels
{
	public class StatusViewModel
	{
		public string MinPriceArticleName { get; set; }
		public decimal MinPrice { get; set; }
		public string MaxStockValueArticleName { get; set; }
		public decimal MaxStockValue { get; set; }
		public int ArticleCount { get; set; }
		public decimal StockValue { get; set; }
		public (int availableInStock, int notAvailableNotinStock, int inStockNotAvailable, int availableNotInStock) StockStatus { get; set; }
	}
}
