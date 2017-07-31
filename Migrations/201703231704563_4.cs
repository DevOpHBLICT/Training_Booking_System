namespace Cascadingdropdownlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Person", "DT");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "DT", c => c.DateTime());
        }
    }
}
