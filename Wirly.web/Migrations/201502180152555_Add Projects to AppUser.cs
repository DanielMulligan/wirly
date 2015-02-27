namespace Wirly.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectstoAppUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Projects");
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id" });
            CreateTable(
                "dbo.AppUserProjects",
                c => new
                    {
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.Project_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Project_Id);
            
            DropColumn("dbo.AspNetUsers", "Project_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Project_Id", c => c.Int());
            DropForeignKey("dbo.AppUserProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.AppUserProjects", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AppUserProjects", new[] { "Project_Id" });
            DropIndex("dbo.AppUserProjects", new[] { "AppUser_Id" });
            DropTable("dbo.AppUserProjects");
            CreateIndex("dbo.AspNetUsers", "Project_Id");
            AddForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Projects", "Id");
        }
    }
}
