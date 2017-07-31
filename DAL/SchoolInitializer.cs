using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Cascadingdropdownlist.Models;

namespace Cascadingdropdownlist.DAL
{
    public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var students = new List<Student>
            {
            new Student{FirstMidName="Carson",LastName="Alexander"},
            new Student{FirstMidName="Meredith",LastName="Alonso"},
            new Student{FirstMidName="Arturo",LastName="Anand"},
            new Student{FirstMidName="Gytis",LastName="Barzdukas"},
            new Student{FirstMidName="Yan",LastName="Li"},
            new Student{FirstMidName="Peggy",LastName="Justice"},
            new Student{FirstMidName="Laura",LastName="Norman"},
            new Student{FirstMidName="Nino",LastName="Olivetto"}
            };

            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();
           
        }
    }
}