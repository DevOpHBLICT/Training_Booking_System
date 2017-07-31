using System;
using System.ComponentModel.DataAnnotations;

namespace Cascadingdropdownlist.ViewModels
{
    public class ReportsByCustomer
    {
        [Key]
        public string Course_DatesID { get; set; }
        public string Organisation { get; set; }
         public string Course { get; set; }
        public int Count { get; set; }
  
    }
}