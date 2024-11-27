using System.ComponentModel.DataAnnotations;

namespace Web_FIA44_ThreeTierArticleApp.Models
{
	public class Article
	{
		[Display(Name = "Artikel-Id")]
		public int AId { get; set; }
		[Display(Name = "Artikelbezeichnung")]
		public string Name { get; set; }
		[Display(Name = "Preis")]
		public decimal Price { get; set; }
		[Display(Name = "Bestand")]
		public int Stock { get; set; }
		[Display(Name = "lieferbar ?")]
		public bool IsAvailable { get; set; }
		[Display(Name = "Status")]
		public string AvailabilityInfo { get; set; }
	}
}
