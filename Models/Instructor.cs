using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cascadingdropdownlist.Models
{
    public class Instructor : Person
    {
        public virtual ICollection<Course_Dates> Course_Dates { get; set; }
    }
}