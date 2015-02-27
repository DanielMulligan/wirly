namespace Wirly.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFilesModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Path = c.String(),
                        Version = c.Int(nullable: false),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Files", new[] { "Project_Id" });
            DropTable("dbo.Files");
        }
    }
}
