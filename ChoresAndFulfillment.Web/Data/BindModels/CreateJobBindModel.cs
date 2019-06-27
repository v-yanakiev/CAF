using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using ChoresAndFulfillment.Models;
namespace ChoresAndFulfillment.Data.BindModels
{
    public class CreateJobBindModel
    {
        [BindRequired]
        [Required]
        public string JobName { get; set; }
        [BindRequired]
        [Required]
        public string Description { get; set; }
        [BindRequired]
        [Required]
        [DataType(DataType.Currency)]
        public decimal Payment { get; set; }
    }
}
