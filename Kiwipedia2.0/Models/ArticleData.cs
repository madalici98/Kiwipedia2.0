using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class ArticleData
    {
        public Article article { get; set; }
        public ArticleVersion articleVersion { get; set; }
        public Category category { get; set; }
    }
}