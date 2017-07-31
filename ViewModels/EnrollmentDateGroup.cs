using System;
using System.ComponentModel.DataAnnotations;

namespace Cascadingdropdownlist.ViewModels
{
    public class EnrollmentDateGroup
    {
        public string Course_DatesID { get; set; }
        public string Venue { get; set; }
        public string Course { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; }

        public string FromHours { get;set; }
        public string FromMinutes { get; set; }
        public string ToHours { get; set; }
        public string ToMinutes { get; set; }
        public int StudentCount { get; set; }
        public int Capacity { get; set; }
        public string Trainer { get; set; }
     }
}