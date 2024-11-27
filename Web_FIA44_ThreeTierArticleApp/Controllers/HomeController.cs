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
			List<Article> articles = _articleService.GetAllArticles(pageNumber, pageSize);
			ViewBag.CurrentPage = pageNumber;
			ViewBag.PageSize = pageSize;
			return View(articles);
		}
		#endregion

		#region DetailAnzeige eines Artikels
		[HttpGet]
		public IActionResult Details(int AId)
		{
			Article article = _articleService.GetArticleById(AId);
			string availibityInfo = _articleService.IsArticleAvailableAndInStock(AId);
			article.AvailabilityInfo = availibityInfo;
			return View(article);
		}
		#endregion

		#region Löschen eines Artikels
		public IActionResult Delete(int AId)
		{
			_articleService.DeleteArticle(AId);
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
			if (ModelState.IsValid)
			{
				_articleService.AddArticle(article);
				return RedirectToAction("Index");
			}
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
			if (ModelState.IsValid)
			{
				_articleService.UpdateArticle(article);
				return RedirectToAction("Index");
			}
			return View(article);

		}
		#endregion

		#region Statustabelle
		public IActionResult Status()
		{
			var minPriceInfo = _articleService.GetArticleMinPrice();
			var maxStockValueInfo = _articleService.GetArticleMaxStockValue();
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
			return View(model);

		}
		#endregion

	}
}
