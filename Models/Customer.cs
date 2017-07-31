using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cascadingdropdownlist.Models
{
    public class Customer
    {
        [Display(Name = "Customer Code")]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        [StringLength(300, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

       
    } }
    
