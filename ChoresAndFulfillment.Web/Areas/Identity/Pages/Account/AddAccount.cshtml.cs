using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoresAndFulfillment.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChoresAndFulfillment.Models;
namespace ChoresAndFulfillment.Areas.Identity.Pages.Account
{
    public class AddAccountModel : PageModel
    {
        private UserManager<User> _userManager;
        private CAFContext _applicationDbContext;

        public AddAccountModel(UserManager<User> userManager, CAFContext applicationDbContext)
        {
            this._userManager = userManager;
            this._applicationDbContext = applicationDbContext;
        }

        public IActionResult OnGet()
        {
            User currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (currentUser.EmployerAccountId != null)
            {
                EmployerAccount employerAccount = 
                    _applicationDbContext.EmployerAccounts.
                    First(a=>a.Id==currentUser.EmployerAccountId);
                employerAccount.UserId = currentUser.Id;
            }
            else
            {
                WorkerAccount workerAccount =
                    _applicationDbContext.WorkerAccounts.
                    First(a => a.Id == currentUser.WorkerAccountId);
                workerAccount.UserId = currentUser.Id;
            }
            _applicationDbContext.SaveChanges();
            return Redirect("/");
        }
    }
}