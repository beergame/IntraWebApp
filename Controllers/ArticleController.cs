using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IntraWebApp.Models.Article;
using IntraWebApp.Services.Models.Article;
using IntraWebApp.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IntraWebApp.Controllers
{
    public class ArticleController : Controller
    {
		private readonly IArticleService _articleService;

		public ArticleController(IArticleService articleService)
		{
			_articleService = articleService;
		}


		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}
        
		[HttpPost]
        [Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				var article = new Article
				{
					Title = model.Title,
					Content = model.Content
				};

                using (var memoryStream = new MemoryStream())
				{
					await model.Picture.CopyToAsync(memoryStream);
					article.Picture = memoryStream.ToArray();
				}
				var accessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData).Value;
				var result = await _articleService.CreateAsync(accessToken, article);
				if (result != 0)
				{
					return RedirectToAction("GetDetails", result);
				}
			}
			return View(model);
		}

		[HttpGet("Article/Details/{id}")]
		public async Task<IActionResult> GetDetails(int id)
		{
			var result = await _articleService.GetByIdAsync(id);
			if (result != null)
			{
				var article = new ArticleDetailsViewModel
                {
                    Id = result.Id,
                    Title = result.Title,
                    Content = result.Content,
                    ImageUrl = GetImageUrl(result.Picture)
                };
				return View(article);
			}
            
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
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
				return View(articles);
			}
			return View();
		}


        private string GetImageUrl(byte[] data)
		{
			var imgToBase64 = Convert.ToBase64String(data);
			return $"data:image/png;base64,{imgToBase64}";
		}

		// GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
