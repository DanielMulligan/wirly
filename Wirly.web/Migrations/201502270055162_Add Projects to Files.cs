namespace Wirly.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectstoFiles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Files", new[] { "Project_Id" });
            CreateTable(
                "dbo.ProjectFiles",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        File_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.File_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Files", t => t.File_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.File_Id);
            
            DropColumn("dbo.Files", "Project_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "Project_Id", c => c.Int());
            DropForeignKey("dbo.ProjectFiles", "File_Id", "dbo.Files");
            DropForeignKey("dbo.ProjectFiles", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectFiles", new[] { "File_Id" });
            DropIndex("dbo.ProjectFiles", new[] { "Project_Id" });
            DropTable("dbo.ProjectFiles");
            CreateIndex("dbo.Files", "Project_Id");
            AddForeignKey("dbo.Files", "Project_Id", "dbo.Projects", "Id");
        }
    }
}
