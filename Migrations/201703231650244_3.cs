namespace Cascadingdropdownlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Course_Dates", "Archived");
            DropColumn("dbo.Course_Dates", "DT");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Course_Dates", "DT", c => c.DateTime(nullable: false));
            AddColumn("dbo.Course_Dates", "Archived", c => c.String());
        }
    }
}
