using System;
namespace IntraWebApp.Business.Models
{
    public class Token
    {
		public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
		public bool IsAdmin { get; set; }
    }
}
