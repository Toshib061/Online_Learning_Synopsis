using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OLS.Models
{
    public class Selection
    {
        [Required(ErrorMessage = "The field cannot be empty")]
        [Display(Name = "Quiz Title:")]
        public string title { set; get; }
        //[Required(ErrorMessage = "The field cannot be empty")]
        //[Display(Name = "Quiz Duration (minutes):")]
        //public int duration { set; get; }
        [Required(ErrorMessage = "The field cannot be empty")]
        [Display(Name = "Quiz Topic:")]
        public int topic_id { set; get; }
        [Required(ErrorMessage = "The field cannot be empty")]
        [Display(Name = "Number of Questions:")]
        public int question_count { set; get; }
    }
}