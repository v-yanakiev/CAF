using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Web.Data.BindModels
{
    public class RatingGivenModel
    {
        [BindRequired]
        [Required]
        public int JobId { get; set; }
        [BindRequired]
        [Required]
        public int Rating { get; set; }
    }
}
