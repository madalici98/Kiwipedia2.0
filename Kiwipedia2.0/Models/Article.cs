using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class Article
    {
        [Key]
        public Guid articleId { get; set; }
        public DateTime creationDate { get; set; }
        [Required]
        public Guid creatorId { get; set; }
        [Required]
        public Guid categoryId { get; set; }
        [Required]
        public string title { get; set; }
        public string description { get; set; }
        public string thumbnail { get; set; }
        [Required]
        public string content { get; set; }
        public DateTime latestEdit { get; set; }

        [ForeignKey("categoryId")]
        public virtual Category Category { set; get; }

        /*[ForeignKey("versionId")]
        public virtual ArticleVersion crrtArticleVersion { set; get; }
        public virtual ICollection<ArticleVersion> ArticleVersions { set; get; }*/
    }

}