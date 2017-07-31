using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cascadingdropdownlist.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentID { get; set; }

        [StringLength(300, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
     
        public int? InstructorID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Instructor Administrator { get; set; }
        public virtual ICollection<Course_Dates> Courses { get; set; }
    }
}