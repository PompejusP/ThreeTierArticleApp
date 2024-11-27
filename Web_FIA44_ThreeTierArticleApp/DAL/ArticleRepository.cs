using Microsoft.Data.SqlClient;
using Web_FIA44_ThreeTierArticleApp.Models;

namespace Web_FIA44_ThreeTierArticleApp.DAL
{
	public class ArticleRepository : IArticleRepository
	{
		private readonly string _connectionString;
		public ArticleRepository(string connectionString)
		{
			_connectionString = connectionString;
		}


		#region Artikel löschen
		public bool DeleteArticle(int AId)
		{
			//SQL Command um einen Artikel zu löschen
			string DeleteQuery = "DELETE FROM Article WHERE AId = @AId";

			//Verbindung zur Datenbank
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				//SQL Command um die Sql-Abfrage auszuführen
				SqlCommand deleteCmd = new SqlCommand(DeleteQuery, connection);
				//Parameterübergabe
				deleteCmd.Parameters.AddWithValue("@AId", AId);
				//Öffnen der Verbindung
				connection.Open();
				//Ausführen der Abfrage
				int rowsAffected = deleteCmd.ExecuteNonQuery();
				//Schließen der Verbindung
				connection.Close();
				//Rückgabe ob ein Artikel gelöscht wurde wenn == 1 dann wurde ein Artikel gelöscht bei == 0 wurde kein Artikel gelöscht
				return rowsAffected == 1;
			}
		}
		#endregion

		#region Alle Artikel anzeigen
		public List<Article> GetAllArticlesByPageAndSize(int pageNumber , int pageSize)
		{
			//SQL Command um alle Artikel zu lesen und zu paginieren
			string paginatedQuery = @"
        SELECT * FROM Article
        ORDER BY AId
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY";
			//die using Anweisung stellt sicher, dass die Verbindung nach dem Verlassen des Blocks geschlossen wird
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				//SQL Command um die Sql-Abfrage auszuführen
				SqlCommand paginatedCmd = new SqlCommand(paginatedQuery, connection);
				//Parameterübergabe für die Paginierung
				paginatedCmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
				paginatedCmd.Parameters.AddWithValue("@PageSize", pageSize);
				//Öffnen der Verbindung
				connection.Open();
				//Ausführen der Abfrage
				SqlDataReader reader = paginatedCmd.ExecuteReader();
				//Artikel Liste erstellen
				List<Article> articles = new List<Article>();
				//Lesen der Daten
				while (reader.Read())
				{
					//Artikel Objekt erstellen
					Article article = new Article
					{
						//Artikel Objekt befüllen
						AId = (int)reader["AId"],
						Name = reader["Name"].ToString(),
						Price = (decimal)reader["Price"],
						Stock = (int)reader["Stock"],
						IsAvailable = (bool)reader["IsAvailable"]
					};
					//Artikel Objekt der Liste hinzufügen
					articles.Add(article);
				}
				//Schließen der Verbindung
				connection.Close();
				//Rückgabe der Artikel Liste
				return articles;
			}
		}
		#endregion

		#region Artikel nach Id anzeigen
		public Article GetArticleById(int AId)
		{
			//SQL Command um einen Artikel nach der Id zu suchen
			string ArticleByIdQuery = "SELECT * FROM Article WHERE AId = @AId";
			//die using Anweisung stellt sicher, dass die Verbindung nach dem Verlassen des Blocks geschlossen wird
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				//SQL Command um die Sql-Abfrage auszuführen
				SqlCommand articleByIdCmd = new SqlCommand(ArticleByIdQuery, connection);
				//Parameterübergabe
				articleByIdCmd.Parameters.AddWithValue("@AId", AId);
				//Öffnen der Verbindung
				connection.Open();
				//Ausführen der Abfrage
				SqlDataReader reader = articleByIdCmd.ExecuteReader();
				//Artikel Objekt erstellen
				Article article = new Article();
				//Lesen der Daten
				if (reader.Read())
				{
					//Artikel Objekt befüllen
					article.AId = (int)reader["AId"];
					article.Name = reader["Name"].ToString();
					article.Price =(decimal)reader["Price"];
					article.Stock = (int)reader["Stock"];
					article.IsAvailable = (bool)reader["IsAvailable"];
				}
				//Schließen der Verbindung
				connection.Close();
				//Rückgabe des Artikels
				return article;
			}
		}
		#endregion

		#region Artikel hinzufügen
		public int InsertArticle(Article article)
		{
			//SQL Command um einen Artikel hinzuzufügen
			string InsertQuery = "INSERT INTO Article (Name, Price, Stock, IsAvailable) output inserted.AId VALUES (@Name, @Price, @Stock, @IsAvailable); SELECT SCOPE_IDENTITY()";
			//die using Anweisung stellt sicher, dass die Verbindung nach dem Verlassen des Blocks geschlossen wird
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				//SQL Command um die Sql-Abfrage auszuführen
				SqlCommand insertCmd = new SqlCommand(InsertQuery, connection);
				//Parameterübergabe
				insertCmd.Parameters.AddWithValue("@Name", article.Name);
				insertCmd.Parameters.AddWithValue("@Price", article.Price);
				insertCmd.Parameters.AddWithValue("@Stock", article.Stock);
				insertCmd.Parameters.AddWithValue("@IsAvailable", article.IsAvailable);
				//Öffnen der Verbindung
				connection.Open();
				//Ausführen der Abfrage mit Rückgabe des neuen Artikel Id
				int newId = (int)insertCmd.ExecuteScalar();
				//Schließen der Verbindung
				connection.Close();
				//Rückgabe der neuen Artikel Id
				return newId;
			}
					
		}			
		#endregion

		#region Artikel bearbeiten
		public bool UpdateArticle(Article article)
		{
			//Sql Command um einen Artikel zu bearbeiten
			string UpdateQuery = "UPDATE Article SET Name = @Name, Price = @Price, Stock = @Stock, IsAvailable = @IsAvailable WHERE AId = @AId";
			//die using Anweisung stellt sicher, dass die Verbindung nach dem Verlassen des Blocks geschlossen wird
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				//SQL Command um die Sql-Abfrage auszuführen
				SqlCommand updateCmd = new SqlCommand(UpdateQuery, connection);
				//Parameterübergabe
				updateCmd.Parameters.AddWithValue("@Name", article.Name);
				updateCmd.Parameters.AddWithValue("@Price", article.Price);
				updateCmd.Parameters.AddWithValue("@Stock", article.Stock);
				updateCmd.Parameters.AddWithValue("@IsAvailable", article.IsAvailable);
				updateCmd.Parameters.AddWithValue("@AId", article.AId);
				//Öffnen der Verbindung
				connection.Open();
				//Ausführen der Abfrage
				int rowsAffected = updateCmd.ExecuteNonQuery();
				//Schließen der Verbindung
				connection.Close();
				//Rückgabe ob ein Artikel bearbeitet wurde wenn == 1 dann wurde ein Artikel bearbeitet bei == 0 wurde kein Artikel bearbeitet
				return rowsAffected == 1;
			}
		}
		#endregion

		#region Alle Artikel lesen
		//diese Methode wurde gebaut um alle Artikel zu lesen ohne Paginierung.
		//diese dient für die Statusanzeige
		public List<Article> GetAllArticles()
		{
			//SQL Command um alle Artikel zu lesen 
			string ReadAllQuery = "SELECT * FROM Article";
			//die using Anweisung stellt sicher, dass die Verbindung nach dem Verlassen des Blocks geschlossen wird
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				//SQL Command um die Sql-Abfrage auszuführen
				SqlCommand readAllCmd = new SqlCommand(ReadAllQuery, connection);
				//Öffnen der Verbindung
				connection.Open();
				//Ausführen der Abfrage
				SqlDataReader reader = readAllCmd.ExecuteReader();
				//Artikel Liste erstellen
				List<Article> articles = new List<Article>();
				//Lesen der Daten
				while (reader.Read())
				{
					//Artikel Objekt erstellen
					Article article = new Article
					{
						//Artikel Objekt befüllen
						AId = (int)reader["AId"],
						Name = reader["Name"].ToString(),
						Price = (decimal)reader["Price"],
						Stock = (int)reader["Stock"],
						IsAvailable = (bool)reader["IsAvailable"]
					};
					//Artikel Objekt der Liste hinzufügen
					articles.Add(article);
				}
				//Schließen der Verbindung
				connection.Close();
				//Rückgabe der Artikel Liste
				return articles;
			}
		}
		#endregion
	}
}
