﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class Category
    {   
        [Key]
        public Guid id { set; get; }
        [Required]
        public string categoryName { set; get; }
    }
}