using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class ArticleVersion
    {   
        [Key]
        public Guid versionId { set; get; }
        [Required]
        public Guid articleId { set; get; }
        public Guid editorId { set; get; } // a user
        public string title { get; set; }
        public string description { get; set; }
        public string thumbnail { get; set; }
        public string content { get; set; }
        public DateTime creationDate { get; set; }

        //public virtual Article article { set; get; }

    }
}