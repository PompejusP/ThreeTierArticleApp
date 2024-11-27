using Web_FIA44_ThreeTierArticleApp.DAL;
using Web_FIA44_ThreeTierArticleApp.Models;

namespace Web_FIA44_ThreeTierArticleApp.BL
{
	public class ArticleService : IArticleService
	{
		private readonly IArticleRepository _articleRepository;

		public ArticleService(IArticleRepository articleRepository)
		{
			_articleRepository = articleRepository;
		}
		#region Artikel hinzufügen
		public void AddArticle(Article article)
		{
			//neuen Artikel erstellen
			Article newArticle = new Article
			{
				//die Eigenschaften des übergebenen Artikels in den neuen Artikel übernehmen
				Name = article.Name,
				Price = article.Price,
				Stock = article.Stock,
				IsAvailable = article.IsAvailable
			};
			//versuchen den Artikel in die Datenbank einzutragen
			try
			{
				_articleRepository.InsertArticle(newArticle);
			}
			//wenn es nicht klappt dann gebe mir eine Fehlermeldung aus
			catch (Exception ex)
			{
				Console.WriteLine($"Fehler beim eintragen des Artikels: {ex.Message}");
			}
		}
		#endregion

		#region Artikel löschen
		public void DeleteArticle(int AId)
		{
			//versuchen den Artikel zu löschen
			try
			{
				_articleRepository.DeleteArticle(AId);
			}
			//wenn es nicht klappt dann gebe mir eine Fehlermeldung aus
			catch (Exception ex)
			{
				Console.WriteLine($"Fehler beim löschen des Artikels: {ex.Message}");
			}
		}
		#endregion

		#region Alle Artikel anzeigen lassen
		public List<Article> GetAllArticles(int pageNumber, int pageSize)
		{
			//alle Artikel aus der Datenbank holen und zurückgeben die auf der Seite angezeigt werden sollen 
			//diese werden dann in der View angezeigt, mit Hilfe der Paginierung werden nur eine bestimmte Anzahl von Artikel
			//gleichzeitig angezeigt

			return _articleRepository.GetAllArticlesByPageAndSize(pageNumber, pageSize);
		}
		#endregion

		#region Artikel nach ID anzeigen lassen
		public Article GetArticleById(int AId)
		{
			//Artikel mit der übergebenen ID aus der Datenbank holen und zurückgeben
			return _articleRepository.GetArticleById(AId);
		}
		#endregion

		#region Artikel mit dem höchsten gesamtwarenpreis anzeigen lassen
		//diese Methode ist gar nicht in der Anwendung implementiert
		public (string Name, decimal StockValue) GetArticleMaxStockValue()
		{
			//alle Artikel aus der Datenbank holen
			var articles = _articleRepository.GetAllArticles();
			if (articles == null || !articles.Any())
			{
				throw new Exception("Keine Artikel vorhanden");
			}
			//Berechne den Warenbastandswert für jeden Artikel und finde den Artikel mit dem höchsten Wert
			var maxStockValueArticle = articles
				.Select(a => new { a.Name, StockValue = a.Price * a.Stock })
				.OrderByDescending(a => a.StockValue)
				.FirstOrDefault();
			//wenn es einen Artikel gibt dann gebe mir den Namen und den Wert zurück
			return (maxStockValueArticle.Name, maxStockValueArticle.StockValue);
		}
		#endregion

		#region Artikel mit dem niedrigsten Preis anzeigen lassen
		public (string Name, decimal Price) GetArticleMinPrice()
		{
			//alle Artikel aus der Datenbank holen
			var articles = _articleRepository.GetAllArticles();
			//wenn es keine Artikel gibt dann gebe mir eine Fehlermeldung aus
			if (articles == null || !articles.Any())
			{
				throw new Exception("Keine Artikel vorhanden");
			}
			//gebe mir den Artikel mit dem niedrigsten Preis
			var minPriceArticle = articles.OrderBy(a => a.Price).FirstOrDefault();
			//wenn es einen Artikel gibt dann gebe mir den Namen und den Preis zurück
			return (minPriceArticle.Name, minPriceArticle.Price);
		}
		#endregion

		#region Gesamtanzahl der Artikel berechnen
		public int GetArticleCount()
		{
			//alle Artikel aus der Datenbank holen und die Anzahl aller in der Datenbank gelisteten Artikel zurückgeben
			return _articleRepository.GetAllArticles().Count;
		}
		#endregion

		#region Gesamtwert des Lagers berechnen
		public decimal GetStockValue()
		{
			//alle Artikel aus der Datenbank holen
			var articles = _articleRepository.GetAllArticles();
			//die Variable für den Gesamtwert des Lagers erstellen und auf 0 setzen
			decimal stockValue = 0;
			//für jeden Artikel den Preis mit dem Bestand multiplizieren und den Wert zu der Variable addieren
			foreach (var article in articles)
			{
				stockValue += article.Price * article.Stock;
			}
			//den Gesamtwert des Lagers zurückgeben
			return stockValue;

		}
		#endregion

		#region Artikel lieferbar  und auf Lager prüfen 
		public (int availableInStock, int notAvailableNotinStock, int inStockNotAvailable, int availableNotInStock) GetStockStatus()
		{
			//alle Artikel aus der Datenbank holen
			var articles = _articleRepository.GetAllArticles();
			//Variablen für die verschiedenen Status erstellen und auf 0 setzen
			int availableInStock = 0;
			int notAvailableNotinStock = 0;
			int inStockNotAvailable = 0;
			int availableNotInStock = 0;

			//für jeden Artikel prüfen ob er lieferbar und auf Lager ist und die entsprechenden Variablen erhöhen
			foreach (var article in articles)
			{
				//wenn der Artikel lieferbar und auf Lager ist dann erhöhe die Variable availableInStock
				if (article.IsAvailable && article.Stock > 0)
				{
					availableInStock++;
				}
				//wenn der Artikel nicht lieferbar und nicht auf Lager ist dann erhöhe die Variable notAvailableNotinStock
				else if (!article.IsAvailable && article.Stock == 0)
				{
					notAvailableNotinStock++;
				}
				//wenn der Artikel lieferbar und nicht auf Lager ist dann erhöhe die Variable inStockNotAvailable
				else if (article.IsAvailable && article.Stock == 0)
				{
					availableNotInStock++;
				}
				//wenn der Artikel nicht lieferbar und auf Lager ist dann erhöhe die Variable inStockNotAvailable
				else if (!article.IsAvailable && article.Stock > 0)
				{
					inStockNotAvailable++;
				}
			}
			//gebe die Anzahl der Artikel mit den verschiedenen Status zurück
			return (availableInStock, notAvailableNotinStock, inStockNotAvailable, availableNotInStock);
		}
		#endregion

		#region Artikel updaten
		public bool UpdateArticle(Article article)
		{
			//neuen Artikel erstellen
			Article newArticle = new Article
			{
				//die Eigenschaften des übergebenen Artikels in den neuen Artikel übernehmen
				AId = article.AId,
				Name = article.Name,
				Price = article.Price,
				Stock = article.Stock,
				IsAvailable = article.IsAvailable
			};
			//versuchen den Artikel in der Datenbank zu updaten
			try
			{
				_articleRepository.UpdateArticle(newArticle);
				return true;
			}
			//wenn es nicht klappt dann gebe mir eine Fehlermeldung aus
			catch (Exception ex)
			{
				Console.WriteLine($"Fehler beim updaten des Artikels: {ex.Message}");
				return false;
			}
		}
		#endregion

		#region Ist Artikel lieferbar und auf Lager
		public string IsArticleAvailableAndInStock(int AId)
		{
			//das Artikel aus der Datenbank holen
			var article = _articleRepository.GetArticleById(AId);
			//wenn der Artikel nicht lieferbar und nicht auf Lager ist dann gebe mir false zurück
			 if (!article.IsAvailable && article.Stock == 0)
			{
				return "Dieser Artikel ist leider nicht mehr verfügbar";
			}
			//wenn der Artikel lieferbar und nicht auf Lager ist dann gebe mir false zur?ck
			else if (article.IsAvailable && article.Stock == 0)
			{
				return "Dieser Artikel kann geliefert werden";
			}
			//wenn der Artikel nicht lieferbar und auf Lager ist dann gebe mir false zurück
			else if (!article.IsAvailable && article.Stock > 0)
			{
				return "Von diesem Artikel sind nur noch Restbestände verfügbar aber ist momentan nicht lieferbar";
			}
			return "Dieser Artikel ist vorrätig";
		}
		#endregion

	}
}

