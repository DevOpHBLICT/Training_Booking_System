using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Cascadingdropdownlist.Models
{
    public class Course
    {
      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Course Code")]
        public int CourseID { get; set; }
   
     
          
    
  
        [Required]
        public string Title { get; set; }
   
      
     }
}