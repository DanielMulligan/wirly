namespace Wirly.web.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using Wirly.web.Infrastructure;
    using Wirly.web.Models;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<Wirly.web.Infrastructure.WirlyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Wirly.web.Infrastructure.WirlyDbContext";
        }

        protected override void Seed(Wirly.web.Infrastructure.WirlyDbContext db)
        {
            
            var adminRole = db.Roles.FirstOrDefault(r => r.Name == "admin");
            if (adminRole == null)
            {
                var rm = new AppRoleManager(new RoleStore<AppRole>(db));
                rm.Create(new AppRole("admin"));
            }

            if (!db.Users.Any(u => u.UserName == "admin"))
            {
                var um = new AppUserManager(new UserStore<AppUser>(db));
                var adminUser = new AppUser { FirstName = "Thomas", LastName = "Andersen", UserName = "admin" };
                um.Create(new AppUser { FirstName = "Thomas", LastName = "Andersen", UserName = "admin" }, "supersecret");
                var newAdminUser = um.FindByName("admin");

                um.AddToRole(newAdminUser.Id, "admin");
            }

            //var file = db.DocumentTypes.FirstOrDefault(d=> d.Name == "file");
            //if(file == null)
            //{
            //    db.DocumentTypes.Add(new DocumentType{Name="file"});
            //}
            
            //var html = db.DocumentTypes.FirstOrDefault(d=> d.Name == "html");
            //if(html == null)
            //{
            //    db.DocumentTypes.Add(new DocumentType{Name="html"});
            //}

            db.SaveChanges();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

        }
    }
}
