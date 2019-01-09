using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class ArticleVersion
    {   
        [Key]
        public Guid versionId { get; set; }
        [Required]
        public Guid articleId { get; set; }
        public Guid editorId { get; set; } 
        public string title { get; set; }
        public string description { get; set; }
        public string thumbnail { get; set; }
        public string content { get; set; }
        public DateTime creationDate { get; set; }

    }
}