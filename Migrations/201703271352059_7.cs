namespace Cascadingdropdownlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Course_Dates", "FromTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Course_Dates", "ToTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Course_Dates", "ToTime");
            DropColumn("dbo.Course_Dates", "FromTime");
        }
    }
}
