namespace Wirly.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStartPagetoAppUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "StartPage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "StartPage");
        }
    }
}
