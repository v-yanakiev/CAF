using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


using ChoresAndFulfillment.Models;
namespace ChoresAndFulfillment.Pages
{
    public class IndexModel : PageModel
    {

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/AccountActionPage/Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
