using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Web.Data.BindModels
{
    public class ApplyForJobBindModel
    {
        [BindRequired]
        [Required]
        public int Id { get; set; }
        [BindRequired]
        [Required]
        [MinLength(5)]
        public string JobApplicationMessage { get; set; }
    }
}
