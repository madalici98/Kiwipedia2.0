using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class NewArticleData
    {
        [Required]
        public string title { get; set; }
        [Required]
        public string category { get; set; }
        public string thumbnail { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string content { get; set; }
    }
}