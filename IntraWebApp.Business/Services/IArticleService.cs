using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IntraWebApp.Business.Models;
using IntraWebApp.Business.Models.Article;

namespace IntraWebApp.Business.Services
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
