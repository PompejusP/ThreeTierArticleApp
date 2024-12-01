using Microsoft.AspNetCore.Mvc;
using Web_FIA44_ThreeTierArticleApp.BL;
using Web_FIA44_ThreeTierArticleApp.Models;
using Web_FIA44_ThreeTierArticleApp.ViewModels;

namespace Web_FIA44_ThreeTierArticleApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly IArticleService _articleService;

		public HomeController(IArticleService articleService)
		{
			_articleService = articleService;
		}

		#region Index Anzeige aller Artikel paginiert
		public IActionResult Index(int pageNumber = 1, int pageSize = 25)
		{
			//Es wird die Anzahl der Artikel ermittelt
			int totalArticles = _articleService.GetArticleCount();
			//Es werden alle Artikel abhängig von der Seitennummer und der Seitenanzahl ermittelt
			List<Article> articles = _articleService.GetAllArticles(pageNumber, pageSize);
			//Die Anzahl der Artikel, die aktuelle Seite, die Seitenanzahl im Viewbag gespeichert und werden an die View übergeben
			ViewBag.CurrentPage = pageNumber;
			ViewBag.PageSize = pageSize;
			ViewBag.TotalPages = (int)Math.Ceiling((double)totalArticles / pageSize);
			//Die Artikel werden an die View übergeben
			return View(articles);
		}
		#endregion

		#region DetailAnzeige eines Artikels
		[HttpGet]
		public IActionResult Details(int AId)
		{
			//es wird der Artikel mit der Artikel-Id AId gesucht
			Article article = _articleService.GetArticleById(AId);
			//Es wird geprüft ob der Artikel verfügbar ist und ob er auf Lager ist für die Anzeige ob dieserArtikel verfügbar ist
			string availibityInfo = _articleService.IsArticleAvailableAndInStock(AId);
			//Der Status wird in das Feld AvailabilityInfo geschrieben
			article.AvailabilityInfo = availibityInfo;
			//Der Artikel wird an die View übergeben
			return View(article);
		}
		#endregion

		#region Löschen eines Artikels
		public IActionResult Delete(int AId)
		{
			//es wird der Artikel mit der Artikel-Id AId gesucht und anschließend gelöscht
			_articleService.DeleteArticle(AId);
			//Nach dem Löschen wird auf die Index Seite zurückgeleitet
			return RedirectToAction("Index");
		}
		#endregion

		#region Erstellen eines Artikels
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Article article)
		{
			//Das Feld AvailabilityInfo wird nicht benötigt und wird aus dem ModelState entfernt dies ist notwendig, da das Feld nicht in der Datenbank vorhanden ist
			//und somit nicht validiert werden kann und somit die ModelState.IsValid Methode immer false zurückgeben würde
			ModelState.Remove("AvailabilityInfo");
			//Prüfen ob die Eingaben korrekt sind
			if (ModelState.IsValid)
			{
				//Artikel hinzufügen und zurück zur Index Seite
				_articleService.AddArticle(article);
				return RedirectToAction("Index");
			}
			//Falls die Eingaben nicht korrekt sind wird die View erneut angezeigt
			return View(article);
		}
		#endregion

		#region Editieren eines Artikels
		[HttpGet]
		public IActionResult Update(int AId)
		{
			Article article = _articleService.GetArticleById(AId);
			return View(article);
		}
		[HttpPost]
		public IActionResult Update(Article article)
		{
			//Das Feld AvailabilityInfo wird nicht benötigt und wird aus dem ModelState entfernt dies ist notwendig, da das Feld nicht in der Datenbank vorhanden ist
			//und somit nicht validiert werden kann und somit die ModelState.IsValid Methode immer false zurückgeben würde
			ModelState.Remove("AvailabilityInfo");
			//Prüfen ob die Eingaben korrekt sind
			if (ModelState.IsValid)
			{
				//Artikel aktualisieren und zurück zur Index Seite
				_articleService.UpdateArticle(article);
				return RedirectToAction("Index");
			}
			//Falls die Eingaben nicht korrekt sind wird die View erneut angezeigt
			return View(article);

		}
		#endregion

		#region Statustabelle
		public IActionResult Status()
		{
			//Es werden die Informationen für die Statustabelle ermittelt
			//Es wird der Artikel mit dem geringsten Preis ermittelt
			var minPriceInfo = _articleService.GetArticleMinPrice();
			//Es wird der Artikel mit dem höchsten Lagerbestand ermittelt
			var maxStockValueInfo = _articleService.GetArticleMaxStockValue();
			//Die Informationen werden in das ViewModel geschrieben und an die View übergeben
			var model = new StatusViewModel
			{
				MinPrice =minPriceInfo.Price,
				MinPriceArticleName = minPriceInfo.Name,
				MaxStockValue = maxStockValueInfo.StockValue,
				MaxStockValueArticleName = maxStockValueInfo.Name,
				ArticleCount = _articleService.GetArticleCount(),
				StockValue = _articleService.GetStockValue(),
				StockStatus = _articleService.GetStockStatus()
			};
			//Die Model wird an die View übergeben
			return View(model);

		}
		#endregion

	}
}
