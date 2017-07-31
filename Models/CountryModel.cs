using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Cascadingdropdownlist.Models
{
    public class CountryModel
    {
        [Required(ErrorMessage = "Country Required.")]
        public int SelectedCountryId { get; set; }
        public System.Web.Mvc.SelectList Countries { get; set; }
    }
}