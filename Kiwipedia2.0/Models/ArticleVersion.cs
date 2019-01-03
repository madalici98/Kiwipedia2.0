using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class ArticleVersion
    {
        public int articleId { set; get; }
        public int articleVersionId { set; get; }
        public int editorId { set; get; }
        public String articlePath { set; get; }
    }
}