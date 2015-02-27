namespace Wirly.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBodytoProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Body", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Body");
        }
    }
}
