using System.Threading.Tasks;
using IntraWebApp.Services.Models;
using IntraWebApp.Services.Models.User;

namespace IntraWebApp.Services.Services
{
	public interface IUserService
    {
		Task<SystemResponse> Create(Register userToRegister);
		Task<Token> Authenticate(Authenticate userToAuthenticate);
		Task<SystemResponse> Update(string accessToken, Update userToUpdate);
		Task<UserInfo> GetUser(string accessToken);
    }
}
