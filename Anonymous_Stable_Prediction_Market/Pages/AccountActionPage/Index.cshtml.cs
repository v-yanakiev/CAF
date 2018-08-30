using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ChoresAndFulfillment.Models;
namespace ChoresAndFulfillment.Pages.AccountActionPage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        public IndexModel(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }
        [Authorize]
        public IActionResult OnGet()
        {
            var currentUser =  _userManager.GetUserAsync(HttpContext.User);
            if(currentUser.Result.AccountType== "WorkerAccount")
            {
                return Redirect("/WorkerManagement/Index");
            }
            else if(currentUser.Result.AccountType== "EmployerAccount")
            {
                return Redirect("/EmployerManagement/Index");
            }
            else
            {
                return Redirect("/");
            }
        }
    }
}