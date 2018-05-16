using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IntraWebApp.Services.Models;
using IntraWebApp.Services.Models.User;
using Newtonsoft.Json;

namespace IntraWebApp.Services.Services
{
	public class UserService : IUserService
    {
		private readonly HttpClient client = new HttpClient();
		private const string apiUrl = "http://172.16.14.140:51650/api/user/";


		public async Task<Token> Authenticate(Authenticate userToAuthenticate)
		{
			Token token = new Token();
			var data = JsonConvert.SerializeObject(userToAuthenticate);
			var body = new StringContent (data, Encoding.UTF8, "application/json");  

			var response = await client.PostAsync(apiUrl + "authenticate", body);

			if (response.IsSuccessStatusCode){
				var content = await response.Content.ReadAsStringAsync();
			    token = JsonConvert.DeserializeObject<Token>(content);
			}
			return token;
		}

		public async Task<SystemResponse> Create(Register userToRegister)
		{
			SystemResponse resSystem = new SystemResponse();
			var data = JsonConvert.SerializeObject(userToRegister);
            var body = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl + "register", body);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
				resSystem = JsonConvert.DeserializeObject<SystemResponse>(content);
            }
			return resSystem;
		}

		public async Task<SystemResponse> Update(string accessToken, Update userToUpdate)
		{
			SystemResponse resSystem = new SystemResponse();
			var data = JsonConvert.SerializeObject(userToUpdate);
            var body = new StringContent(data, Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("access_token", accessToken);

			var response = await client.PutAsync(apiUrl + "update", body);

            if (response.IsSuccessStatusCode)
            {
				resSystem = SystemResponse.Success;
            }
            return resSystem;
		}

		public async Task<UserInfo> GetUser(string accessToken)
		{
			var userInfo = new UserInfo();
			client.DefaultRequestHeaders.Add("access_token", accessToken);
			var response = await client.GetAsync(apiUrl + "getUser");
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				userInfo = JsonConvert.DeserializeObject<UserInfo>(content);
			}
			return userInfo;
		}
	}
}
