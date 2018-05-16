using System;
namespace IntraWebApp.Services.Models
{
    public class Token
    {
		public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
    }
}
