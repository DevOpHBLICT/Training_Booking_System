using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cascadingdropdownlist.Models
{
    public class Student : Person
    {
        /*
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        */
        [Display (Name="Job Title")]
        [StringLength(50, ErrorMessage = "Job Title cannot be longer than 50 characters.")]
        public string Job_Title { get; set; }

  
        [StringLength(50, ErrorMessage = "Item cannot be longer than 50 characters.")]
        [Required (ErrorMessage="Contact Telephone is required")]
        [Display (Name="*Contact Tel:")]
        public string Contact_Tel_No { get; set; }

        [StringLength(50, ErrorMessage = "Item cannot be longer than 50 characters.")]
        [Required(ErrorMessage ="Work Base is Required")]
        [Display (Name ="*Work Base:")]
        public string Work_Base_Address { get; set; }
        [StringLength(50, ErrorMessage = "Item cannot be longer than 50 characters.")]

        [Display(Name = "Mobile Tel:")]
        public string Mobile_No { get; set; }
        [StringLength(50, ErrorMessage = "Item cannot be longer than 50 characters.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "*Email:")]
        [Required (ErrorMessage ="Email is required")]
        public string Email_Address { get; set; }
          [Required (ErrorMessage ="Manager's Name Required")]
        [Display(Name = "*Managers Name:")]
        public string Managers_Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Managers Date:")]
        public string Managers_Date { get; set; }
        [StringLength(50, ErrorMessage = "Item cannot be longer than 50 characters.")]
        [Display(Name = "Print Managers Name:")]
        public string Print_Managers_Name { get; set; }
        [StringLength(50, ErrorMessage = "Item cannot be longer than 50 characters.")]
      
        [Display(Name = "* Organisation:")]
         public string Organisation { get; set; }

        [Display(Name = "Student ID:")]
        public int? StudentID { get; set; }
        public int? CourseID { get; set; }
       [Display(Name = "* Course Name:")]
        public string Course_Name { get; set; }
         [Display(Name = "* Start Date:")]
         public DateTime? DF { get; set; }
    
        [Display(Name = "* Venue:")]
         public string Venue { get; set; }

        [DisplayFormat(NullDisplayText = "?")]
        [Display(Name = "Attended")]
        public string Attended { get; set; }


      

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }


    }
}