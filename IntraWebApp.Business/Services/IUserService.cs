using System.Threading.Tasks;
using IntraWebApp.Business.Models;
using IntraWebApp.Business.Models.User;

namespace IntraWebApp.Business.Services
{
	public interface IUserService
    {
		Task<SystemResponse> Create(Register userToRegister);
		Task<Token> Authenticate(Authenticate userToAuthenticate);
		Task<SystemResponse> Update(string accessToken, Update userToUpdate);
		Task<UserInfo> GetUser(string accessToken);
    }
}
