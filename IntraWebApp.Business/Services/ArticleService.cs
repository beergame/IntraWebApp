﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IntraWebApp.Business.Models;
using IntraWebApp.Business.Models.Article;
using Newtonsoft.Json;

namespace IntraWebApp.Business.Services
{
	public class ArticleService : IArticleService
	{
		private readonly HttpClient client = new HttpClient();
		private const string apiUrl = "http://172.16.67.242:51650/api/article/";

		public async Task<int> CreateAsync(string accessToken, Article article)
		{
			int articleId = 0;
			var data = JsonConvert.SerializeObject(article);
			var body = new StringContent(data, Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("access_token", accessToken);

			var response = await client.PostAsync(apiUrl + "create", body);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				articleId = JsonConvert.DeserializeObject<int>(content);
			}
			return articleId;
		}

		public async Task<SystemResponse> DeleteByIdAsync(string accessToken, int id)
		{
			SystemResponse systemResponse = new SystemResponse();

            client.DefaultRequestHeaders.Add("access_token", accessToken);

			var response = await client.DeleteAsync(apiUrl + "delete/" + id);
			switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    systemResponse = SystemResponse.AccessDenied;
                    break;
                case HttpStatusCode.NotFound:
                    systemResponse = SystemResponse.NotFound;
                    break;
                case HttpStatusCode.OK:
                    systemResponse = SystemResponse.Success;
                    break;
                default:
                    break;
            }
			return systemResponse;
		}

		public async Task<IEnumerable<Article>> GetAllAsync()
		{
			var articles = new List<Article>();
			var response = await client.GetAsync(apiUrl + "getAll?pageSize=5");
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				articles = JsonConvert.DeserializeObject<List<Article>>(content);
			}
			return articles;
		}

		public async Task<Article> GetByIdAsync(int id)
		{

			Article article = new Article();
            
			var response = await client.GetAsync(apiUrl + "getById/" + id);
            if (response.IsSuccessStatusCode)
            {
				var content = await response.Content.ReadAsStringAsync();
				article = JsonConvert.DeserializeObject<Article>(content);                
            }
			return article;
		}

		public async Task<SystemResponse> UpdateAsync(string accessToken, UpdateArticle article)
		{
			SystemResponse systemResponse = new SystemResponse();

			var data = JsonConvert.SerializeObject(article);
            var body = new StringContent(data, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("access_token", accessToken);

			var response = await client.PutAsync(apiUrl + "update", body);

			switch (response.StatusCode)
			{
				case HttpStatusCode.Unauthorized :
					systemResponse = SystemResponse.AccessDenied;
					break;
				case HttpStatusCode.NotFound :
					systemResponse = SystemResponse.NotFound;
					break;
				case HttpStatusCode.OK :
					systemResponse = SystemResponse.Success;
					break;
				default:
					break;
			}

            return systemResponse;
		}
	}
}
