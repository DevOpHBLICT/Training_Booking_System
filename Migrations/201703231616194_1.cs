namespace Cascadingdropdownlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Course", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Course", "Description", c => c.String(nullable: false));
        }
    }
}
