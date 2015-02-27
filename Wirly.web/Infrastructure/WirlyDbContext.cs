using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wirly.web.Models;
using System.Data.Entity;

namespace Wirly.web.Infrastructure
{
    public class WirlyDbContext : IdentityDbContext<AppUser>
    {
        public WirlyDbContext() : base("WirlyDb") { }

        static WirlyDbContext()
        {
            //this just specifies a class that will seed the db when the schema is frst created
            //Database.SetInitializer<WirlyDbContext>(new WirlyDbInit());

        }

        //This is how instances of this class will be created when needed by Owin
        public static WirlyDbContext Create()
        {
            return new WirlyDbContext();
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<File> Files { get; set; }
    }
}