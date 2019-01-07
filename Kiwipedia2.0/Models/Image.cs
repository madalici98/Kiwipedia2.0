using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class Image
    {   
        [Key]
        public Guid id;
        [Required]
        public Guid versionId;
        [Required]
        public String source;
    }

    public class ImageDBContext : DbContext
    {
        public ImageDBContext() : base("DBConnectionString") { }
        public DbSet<Image> Images { get; set; }
    }
}