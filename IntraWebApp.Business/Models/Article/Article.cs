using System;
namespace IntraWebApp.Business.Models.Article
{
    public class Article
    {
		public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
		public DateTime CreationDate { get; set; }
        public byte[] Picture { get; set; }
    }
}
