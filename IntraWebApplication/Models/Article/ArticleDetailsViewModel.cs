using System;
namespace IntraWebApp.Models.Article
{
    public class ArticleDetailsViewModel
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string ImageUrl { get; set; }
		public DateTime CreationDate { get; set; }
    }
}
