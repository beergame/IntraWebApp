using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntraWebApp.Services.Services;
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

		// GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
