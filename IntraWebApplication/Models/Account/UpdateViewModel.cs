using System;
using System.ComponentModel.DataAnnotations;

namespace IntraWebApp.Models.Account
{
    public class UpdateViewModel
    {
		public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

		[DataType(DataType.Password)]
        [Display(Name = "New Password")]
		public string NewPassword
        {
            get;
            set;
        }

		[DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmNewPassword
        {
            get;
            set;
        }
    }
}
