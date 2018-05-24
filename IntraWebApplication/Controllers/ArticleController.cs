using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IntraWebApp.Business.Models;
using IntraWebApp.Business.Models.Article;
using IntraWebApp.Business.Services;
using IntraWebApp.Models.Article;
using IntraWebApplication.Models.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IntraWebApplication.Controllers
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

			    if (model.Picture != null)
			    {
			        using (var memoryStream = new MemoryStream())
			        {
			            await model.Picture.CopyToAsync(memoryStream);
			            article.Picture = memoryStream.ToArray();
			        }
			    }
                var accessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
				var result = await _articleService.CreateAsync(accessToken, article);
				if (result != 0)
				{
					return RedirectToAction("GetAll");
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

        [HttpGet("Article/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _articleService.GetByIdAsync(id);
            if (result != null)
            {
                var article = new ArticleDetailsViewModel
                {
                    Id = result.Id,
                    Title = result.Title,
                    Content = result.Content
                };
                return View(article);
            }

            return View();
        }

		[HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var accessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
			var result = await _articleService.DeleteByIdAsync(accessToken, id);
			if (result == SystemResponse.Success)
			{
				return RedirectToAction("GetAll");
			}

			return RedirectToAction("GetDetails", new {id = id});
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
            if (data == null) return string.Empty;
			var imgToBase64 = Convert.ToBase64String(data);
			return $"data:image/png;base64,{imgToBase64}";
		}

        [HttpGet("Article/update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _articleService.GetByIdAsync(id);
            if (result == null) return View();

            var article = new ArticleUpdateViewModel
            {
                Id = result.Id,
                Title = result.Title,
                Content = result.Content
            };
            return View(article);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ArticleUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = new UpdateArticle
                {
                    Id = id,
                    Title = model.Title,
                    Content = model.Content
                };

                if (model.Picture != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Picture.CopyToAsync(memoryStream);
                        article.Picture = memoryStream.ToArray();
                    }
                }
                
                var accessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
                var result = await _articleService.UpdateAsync(accessToken, article);
                if (result == SystemResponse.Success)
                {
                    return RedirectToAction("GetDetails", new {id = id});
                }
            }
            return View(model);
        }

		// GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
