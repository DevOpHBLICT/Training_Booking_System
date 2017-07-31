using System;
using System.ComponentModel.DataAnnotations;

namespace Cascadingdropdownlist.ViewModels
{
    public class ReportsByCustomer
    {
        [Key]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DF { get; set; }

        public int Course_DatesID { get; set; }


        public string Dept { get; set; }

        public int Duration { get; set; }
        public string Organisation { get; set; }


        public string Course_Title { get; set; }


    }
}