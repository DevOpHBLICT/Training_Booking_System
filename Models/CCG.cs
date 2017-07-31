using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cascadingdropdownlist.Models
{
    public class CCG
    {
        [Display(Name = "CCG Code")]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CCGID { get; set; }

        [StringLength(300, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

       
    } }
    
