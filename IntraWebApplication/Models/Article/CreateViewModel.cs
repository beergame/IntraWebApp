using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace IntraWebApp.Models.Article
{
    public class CreateViewModel
    {
		[Required]
		public string Title { get; set; }
		[Required]
        public string Content { get; set; }
        public IFormFile Picture { get; set; }
    }
}
