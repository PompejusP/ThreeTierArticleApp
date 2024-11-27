using Web_FIA44_ThreeTierArticleApp.BL;
using Web_FIA44_ThreeTierArticleApp.DAL;

namespace Web_FIA44_ThreeTierArticleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("SqlServer");

            builder.Services.AddScoped<IArticleRepository>(provider => new ArticleRepository(connectionString));
			builder.Services.AddScoped<IArticleService, ArticleService>();

			var app = builder.Build();

            app.MapControllerRoute(name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
			app.UseAuthorization();
			app.UseStaticFiles();

            app.UseRouting();
            app.Run();
        }
    }
}
