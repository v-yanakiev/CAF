using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChoresAndFulfillment.Data.ViewModels
{
    public class CreateJobViewModel
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
        [Range(1,99999)]
        public decimal Payment { get; set; }
    }
}
