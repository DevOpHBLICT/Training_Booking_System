namespace Cascadingdropdownlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ReportsByCCG", newName: "ReportsByCustomer");
            DropForeignKey("dbo.CCG", "Student_ID", "dbo.Person");
            DropIndex("dbo.CCG", new[] { "Student_ID" });
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 300),
                        Student_ID = c.Int(),
                    })
                .PrimaryKey(t => t.CustomerID)
                .ForeignKey("dbo.Person", t => t.Student_ID)
                .Index(t => t.Student_ID);
            
            DropTable("dbo.CCG");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CCG",
                c => new
                    {
                        CCGID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 300),
                        Student_ID = c.Int(),
                    })
                .PrimaryKey(t => t.CCGID);
            
            DropForeignKey("dbo.Customer", "Student_ID", "dbo.Person");
            DropIndex("dbo.Customer", new[] { "Student_ID" });
            DropTable("dbo.Customer");
            CreateIndex("dbo.CCG", "Student_ID");
            AddForeignKey("dbo.CCG", "Student_ID", "dbo.Person", "ID");
            RenameTable(name: "dbo.ReportsByCustomer", newName: "ReportsByCCG");
        }
    }
}
