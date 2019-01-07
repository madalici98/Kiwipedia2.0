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
        public Guid id { get; set; }
        [Required]
        public Guid currentVersionId { get; set; }
        public DateTime creationDate { get; set; }
        [Required]
        public Guid creatorId { get; set; }
        [Required]
        public Guid categoryId { get; set; }
        [ForeignKey("currentVersionId")]
        public ArticleVersion crrtArticleVersion { set; get; }
        //public virtual ICollection<ArticleVersion> ArticleVersions { set; get; }
        [ForeignKey("categoryId")]
        public Category category { set; get; }
    }

}