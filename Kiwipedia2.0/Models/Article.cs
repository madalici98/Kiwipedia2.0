using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public virtual ICollection<ArticleVersion> ArticleVersions { set; get; }
    }

    public class ArticleDBContext : DbContext
    {
        public ArticleDBContext() : base("DBConnectionString") { }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleVersion> ArticleVersions { get; set; }
    }

}