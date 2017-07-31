using System.Collections.Generic;
using Cascadingdropdownlist.Models;

namespace Cascadingdropdownlist.ViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course_Dates> Course_Dates { get; set; }
         public IEnumerable<Student> Students { get; set; }

    }
}

