using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kiwipedia2._0.Models
{
    public class User
    {   
        public Guid id { set; get; }
        public string username { set; get; }
        public string email { set; get; }
    }

    /*public class UserDBContext : DbContext
    {
        public UserDBContext() : base ("DBConnectionString") { }
        public DbSet<User> Users {get; set;}
    }*/
}