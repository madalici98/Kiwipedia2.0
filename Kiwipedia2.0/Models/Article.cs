using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class Article
    {
        public int articleId { set; get; }
        public String category { set; get; }
        public int author { set; get; }
        public int currtVersion { set; get; }
    }
}