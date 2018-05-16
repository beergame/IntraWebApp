﻿using System;
namespace IntraWebApp.Services.Models.Article
{
    public class UpdateArticle
    {
		public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] Picture { get; set; }
    }
}
