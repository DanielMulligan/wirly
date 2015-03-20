namespace Wirly.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentTypetoDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "DocumentType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "DocumentType");
        }
    }
}
