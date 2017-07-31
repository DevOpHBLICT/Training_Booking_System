using Cascadingdropdownlist.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Cascadingdropdownlist.DAL
{
    public class SchoolContext : DbContext
    {
        public DbSet<Course_Dates> Course_Dates { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Customer> Customers { get; set; }

     
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

         
            modelBuilder.Entity<Course_Dates>()
                
                .HasMany(c => c.Instructors).WithMany(i => i.Course_Dates)
                .Map(t => t.MapLeftKey("CourseID")
                    .MapRightKey("InstructorID")
                    .ToTable("CourseInstructor"));




            modelBuilder.Entity<Department>().MapToStoredProcedures();
        }

        public System.Data.Entity.DbSet<Cascadingdropdownlist.ViewModels.ReportsByCustomer> ReportsByCustomers { get; set; }
    }
}