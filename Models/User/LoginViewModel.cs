using System.ComponentModel.DataAnnotations;

namespace IntraWebApp.Models.User
{
	public class LoginViewModel
    {
		[Required]
        public string Username
        {
            get;
            set;
        }

        [Required, DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password
        {
            get;
            set;
        }
    }
}
