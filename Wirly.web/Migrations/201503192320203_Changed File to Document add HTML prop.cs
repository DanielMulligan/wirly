namespace Wirly.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedFiletoDocumentaddHTMLprop : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Files", newName: "Documents");
            AddColumn("dbo.Documents", "Html", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "Html");
            RenameTable(name: "dbo.Documents", newName: "Files");
        }
    }
}
