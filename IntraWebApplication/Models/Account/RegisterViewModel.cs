using System;
using System.ComponentModel.DataAnnotations;

namespace IntraWebApp.Models.Account
{
    public class RegisterViewModel
    {
		[Required]
		public string Civility
        {
            get;
            set;
        }

		[Required]
        public string FirstName
        {
            get;
            set;
        }

		[Required]
        public string LastName
        {
            get;
            set;
        }

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

		[Required, DataType(DataType.Password)] 
		[Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword
		{
			get;
			set;
		}
	}
}
