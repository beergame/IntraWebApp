using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IntraWebApp.Services.Models;
using IntraWebApp.Services.Models.Article;

namespace IntraWebApp.Services.Services
{
    public interface IArticleService
    {
		Task<Article> GetByIdAsync(int id);
		Task<IEnumerable<Article>> GetAllAsync();
		Task<SystemResponse> DeleteByIdAsync(string accessToken, int id);
		Task<int> CreateAsync(string accessToken, Article article);
		Task<SystemResponse> UpdateAsync(string accessToken, UpdateArticle article);
    }
}
