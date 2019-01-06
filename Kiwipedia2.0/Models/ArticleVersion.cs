using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class ArticleVersion
    {
        public string versionId { set; get; }
        public string articleId { set; get; }
        public string editorId { set; get; }
        public string title { get; set; }
        public string description { get; set; }
        public string thumbnail { get; set; }
        public string content { get; set; }
        public DateTime creationDate { get; set; }
    }
}