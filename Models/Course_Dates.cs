using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cascadingdropdownlist.Models
{
    public class Course_Dates
    {

        [Display(Name = "Full Course")]
        public string FullCourse
        {
            get
            {
                return "Course ID:" + CourseID + " Title:" + Course_Title + " Location:" + Dept + " Course Starts:" + DF;
            }

        }

        [Key]
        [Required]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Course_DatesID { get; set; }

        [Display(Name = "Maximum Places")]
        [Required]
        public int Capacity { get; set; }


        [Required]
        [ForeignKey("Course")]
        public int CourseID { get; set; }



        [Display(Name = "Start Date dd/mm/yyyy")]
        [Required(ErrorMessage = "Please enter Start Date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DF { get; set; }




        [Required]
     [Display(Name = "From Hours")]
        public int FromHours { get; set; }
        [Required]
     [Display(Name = "From Minutes")]
        public int FromMinutes { get; set; }
        [Required]
     [Display(Name = "To Hours")]
        public int ToHours { get; set; }
        [Required]
     [Display(Name = "To Minutes")]
        public int ToMinutes { get; set; }

        public int Duration { get; set; }



        public string Course_Title
        {
            get; set;
        }
       [Display(Name = "Venue")]
        public string Dept
        { get; set; }

       [Display(Name = "Venue")]
        public int DepartmentID { get; set; }
        public virtual ICollection<Course> Course { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<Department> Department { get; set; }

    }



}