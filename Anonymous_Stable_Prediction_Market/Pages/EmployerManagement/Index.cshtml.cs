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
namespace ChoresAndFulfillment.Pages.EmployerManagement
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly CAFContext _applicationDbContext;
        public IndexModel(UserManager<User> userManager, CAFContext applicationDbContext)
        {
            this._userManager = userManager;
            this._applicationDbContext = applicationDbContext;
        }
        [Authorize]
        public IActionResult OnGet()
        {
            if (IsWorker())
            {
                return Redirect("/WorkerManagement/Index");
            }
            return Page();
        }
        private bool IsWorker()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (currentUser.AccountType == "EmployerAccount")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}