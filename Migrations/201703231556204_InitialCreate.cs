namespace Cascadingdropdownlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
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
            
            CreateTable(
                "dbo.Course_Dates",
                c => new
                    {
                        Course_DatesID = c.Int(nullable: false, identity: true),
                        Archived = c.String(),
                        Capacity = c.Int(nullable: false),
                        DT = c.DateTime(nullable: false),
                        CourseID = c.Int(nullable: false),
                        DF = c.DateTime(nullable: false),
                        Course_Title = c.String(),
                        Dept = c.String(),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Course_DatesID);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        CourseID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 300),
                        Description = c.String(nullable: false),
                        Start_Time = c.String(nullable: false),
                        Course_Dates_Course_DatesID = c.Int(),
                        Student_ID = c.Int(),
                    })
                .PrimaryKey(t => t.CourseID)
                .ForeignKey("dbo.Course_Dates", t => t.Course_Dates_Course_DatesID)
                .ForeignKey("dbo.Person", t => t.Student_ID)
                .Index(t => t.Course_Dates_Course_DatesID)
                .Index(t => t.Student_ID);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 300),
                        InstructorID = c.Int(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DepartmentID)
                .ForeignKey("dbo.Person", t => t.InstructorID)
                .Index(t => t.InstructorID);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Job_Title = c.String(maxLength: 50),
                        Contact_Tel_No = c.String(maxLength: 50),
                        Work_Base_Address = c.String(maxLength: 50),
                        Mobile_No = c.String(maxLength: 50),
                        Email_Address = c.String(maxLength: 50),
                        Managers_Name = c.String(),
                        Managers_Date = c.String(),
                        Print_Managers_Name = c.String(maxLength: 50),
                        Organisation = c.String(maxLength: 50),
                        StudentID = c.Int(),
                        CourseID = c.Int(),
                        Course_Name = c.String(),
                        DF = c.DateTime(),
                        DT = c.DateTime(),
                        Venue = c.String(),
                        Attended = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ReportsByCustomer",
                c => new
                    {
                        Course_DatesID = c.String(nullable: false, maxLength: 128),
                        Organisation = c.String(),
                        Course = c.String(),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Course_DatesID);
            
            CreateTable(
                "dbo.DepartmentCourse_Dates",
                c => new
                    {
                        Department_DepartmentID = c.Int(nullable: false),
                        Course_Dates_Course_DatesID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_DepartmentID, t.Course_Dates_Course_DatesID })
                .ForeignKey("dbo.Department", t => t.Department_DepartmentID, cascadeDelete: true)
                .ForeignKey("dbo.Course_Dates", t => t.Course_Dates_Course_DatesID, cascadeDelete: true)
                .Index(t => t.Department_DepartmentID)
                .Index(t => t.Course_Dates_Course_DatesID);
            
            CreateTable(
                "dbo.CourseInstructor",
                c => new
                    {
                        CourseID = c.Int(nullable: false),
                        InstructorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseID, t.InstructorID })
                .ForeignKey("dbo.Course_Dates", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.InstructorID, cascadeDelete: true)
                .Index(t => t.CourseID)
                .Index(t => t.InstructorID);
            
            CreateStoredProcedure(
                "dbo.Department_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 300),
                        InstructorID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Department]([Name], [InstructorID])
                      VALUES (@Name, @InstructorID)
                      
                      DECLARE @DepartmentID int
                      SELECT @DepartmentID = [DepartmentID]
                      FROM [dbo].[Department]
                      WHERE @@ROWCOUNT > 0 AND [DepartmentID] = scope_identity()
                      
                      SELECT t0.[DepartmentID], t0.[RowVersion]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );
            
            CreateStoredProcedure(
                "dbo.Department_Update",
                p => new
                    {
                        DepartmentID = p.Int(),
                        Name = p.String(maxLength: 300),
                        InstructorID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"UPDATE [dbo].[Department]
                      SET [Name] = @Name, [InstructorID] = @InstructorID
                      WHERE (([DepartmentID] = @DepartmentID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );
            
            CreateStoredProcedure(
                "dbo.Department_Delete",
                p => new
                    {
                        DepartmentID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Department]
                      WHERE (([DepartmentID] = @DepartmentID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Department_Delete");
            DropStoredProcedure("dbo.Department_Update");
            DropStoredProcedure("dbo.Department_Insert");
            DropForeignKey("dbo.Course", "Student_ID", "dbo.Person");
            DropForeignKey("dbo.Customer", "Student_ID", "dbo.Person");
            DropForeignKey("dbo.CourseInstructor", "InstructorID", "dbo.Person");
            DropForeignKey("dbo.CourseInstructor", "CourseID", "dbo.Course_Dates");
            DropForeignKey("dbo.DepartmentCourse_Dates", "Course_Dates_Course_DatesID", "dbo.Course_Dates");
            DropForeignKey("dbo.DepartmentCourse_Dates", "Department_DepartmentID", "dbo.Department");
            DropForeignKey("dbo.Department", "InstructorID", "dbo.Person");
            DropForeignKey("dbo.Course", "Course_Dates_Course_DatesID", "dbo.Course_Dates");
            DropIndex("dbo.CourseInstructor", new[] { "InstructorID" });
            DropIndex("dbo.CourseInstructor", new[] { "CourseID" });
            DropIndex("dbo.DepartmentCourse_Dates", new[] { "Course_Dates_Course_DatesID" });
            DropIndex("dbo.DepartmentCourse_Dates", new[] { "Department_DepartmentID" });
            DropIndex("dbo.Department", new[] { "InstructorID" });
            DropIndex("dbo.Course", new[] { "Student_ID" });
            DropIndex("dbo.Course", new[] { "Course_Dates_Course_DatesID" });
            DropIndex("dbo.Customer", new[] { "Student_ID" });
            DropTable("dbo.CourseInstructor");
            DropTable("dbo.DepartmentCourse_Dates");
            DropTable("dbo.ReportsByCustomer");
            DropTable("dbo.Person");
            DropTable("dbo.Department");
            DropTable("dbo.Course");
            DropTable("dbo.Course_Dates");
            DropTable("dbo.Customer");
        }
    }
}
