using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text.RegularExpressions;
 namespace Cascadingdropdownlist.Models
 { 
public class RegistrationModel 
{
 [Required(ErrorMessage = "Please Enter Email Address")]
 [Display(Name = "UserName (Email Address)")]
 [RegularExpression(".+@.+\\..+", ErrorMessage = "Please Enter Correct Email Address")]
 public string UserName { get; set; }
 [Display(Name = "Country")]
 public Country Country { get; set; }
 [Display(Name = "City")]
 public City City { get; set; }
 [Required(ErrorMessage = "Please Enter Address")]
 [Display(Name = "Address")]
 [StringLength(200)]
 public string Address { get; set; }
 }
// IClientValidatable for client side Validation 
 public class MustBeSelectedAttribute : ValidationAttribute, IClientValidatable 
{
 public override bool IsValid(object value) {
 if (value == null || (int)value == 0)
 return false;
 else return true; 
} 
// Implement IClientValidatable for client side Validation 
public IEnumerable GetClientValidationRules(ModelMetadata metadata, ControllerContext context) 
{ 
return new ModelClientValidationRule[] 
{
 new ModelClientValidationRule { ValidationType = "dropdown", ErrorMessage = this.ErrorMessage } }; 
} 
}
 public class Country
 { 
[MustBeSelectedAttribute(ErrorMessage = "Please Select Country")] 
public int? ID { get; set; }
 public string Name { get; set; }
 }
 public class City 
{ 
[MustBeSelectedAttribute(ErrorMessage = "Please Select City")] 
public int? ID { get; set; } 
public string Name { get; set; }
 public int? Country { get; set; } 
}
} 
