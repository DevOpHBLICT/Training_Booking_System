namespace Cascadingdropdownlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Course", "Course_Dates_CourseID", "dbo.Course_Dates");
            DropForeignKey("dbo.DepartmentCourse_Dates", "Course_Dates_CourseID", "dbo.Course_Dates");
            DropForeignKey("dbo.CourseInstructor", "CourseID", "dbo.Course_Dates");
            RenameColumn(table: "dbo.Course", name: "Course_Dates_CourseID", newName: "Course_Dates_Course_DatesID");
            RenameColumn(table: "dbo.DepartmentCourse_Dates", name: "Course_Dates_CourseID", newName: "Course_Dates_Course_DatesID");
            RenameIndex(table: "dbo.Course", name: "IX_Course_Dates_CourseID", newName: "IX_Course_Dates_Course_DatesID");
            RenameIndex(table: "dbo.DepartmentCourse_Dates", name: "IX_Course_Dates_CourseID", newName: "IX_Course_Dates_Course_DatesID");
            DropPrimaryKey("dbo.Course_Dates");
            AddPrimaryKey("dbo.Course_Dates", "Course_DatesID");
            AddForeignKey("dbo.Course", "Course_Dates_Course_DatesID", "dbo.Course_Dates", "Course_DatesID");
            AddForeignKey("dbo.DepartmentCourse_Dates", "Course_Dates_Course_DatesID", "dbo.Course_Dates", "Course_DatesID", cascadeDelete: true);
            AddForeignKey("dbo.CourseInstructor", "CourseID", "dbo.Course_Dates", "Course_DatesID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseInstructor", "CourseID", "dbo.Course_Dates");
            DropForeignKey("dbo.DepartmentCourse_Dates", "Course_Dates_Course_DatesID", "dbo.Course_Dates");
            DropForeignKey("dbo.Course", "Course_Dates_Course_DatesID", "dbo.Course_Dates");
            DropPrimaryKey("dbo.Course_Dates");
            AddPrimaryKey("dbo.Course_Dates", "CourseID");
            RenameIndex(table: "dbo.DepartmentCourse_Dates", name: "IX_Course_Dates_Course_DatesID", newName: "IX_Course_Dates_CourseID");
            RenameIndex(table: "dbo.Course", name: "IX_Course_Dates_Course_DatesID", newName: "IX_Course_Dates_CourseID");
            RenameColumn(table: "dbo.DepartmentCourse_Dates", name: "Course_Dates_Course_DatesID", newName: "Course_Dates_CourseID");
            RenameColumn(table: "dbo.Course", name: "Course_Dates_Course_DatesID", newName: "Course_Dates_CourseID");
            AddForeignKey("dbo.CourseInstructor", "CourseID", "dbo.Course_Dates", "CourseID", cascadeDelete: true);
            AddForeignKey("dbo.DepartmentCourse_Dates", "Course_Dates_CourseID", "dbo.Course_Dates", "CourseID", cascadeDelete: true);
            AddForeignKey("dbo.Course", "Course_Dates_CourseID", "dbo.Course_Dates", "CourseID");
        }
    }
}
