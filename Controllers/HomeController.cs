using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IntraWebApp.Models;
using IntraWebApp.Services.Services;
using IntraWebApp.Models.Article;

namespace IntraWebApp.Controllers
{
    public class HomeController : Controller
    {
		private readonly IArticleService _articleService;

		public HomeController(IArticleService articleService)
		{
			_articleService = articleService;
		}
       

		private string GetImageUrl(byte[] data)
        {
            var imgToBase64 = Convert.ToBase64String(data);
            return $"data:image/png;base64,{imgToBase64}";
        }

		[HttpGet]
		public async Task<IActionResult> Index()
        {
			var result = await _articleService.GetAllAsync();
            if (result != null)
            {
                var articles = new List<ArticleDetailsViewModel>();
                foreach (var item in result)
                {
                    var article = new ArticleDetailsViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Content = item.Content,
                        ImageUrl = GetImageUrl(item.Picture),
                        CreationDate = item.CreationDate
                    };
                    articles.Add(article);
                }
                articles.OrderBy(a => a.CreationDate);
                return View(articles.TakeLast(4));
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
