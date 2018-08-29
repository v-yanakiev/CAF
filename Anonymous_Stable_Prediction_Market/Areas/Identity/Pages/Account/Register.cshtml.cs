using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ChoresAndFulfillment.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
namespace ChoresAndFulfillment.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Type of account")]
            [Required]
            [DataType(DataType.Text)]
            public string TypeOfAccount { get; set; }

            [Display(Name = "Telephone number")]
            [Required]
            [DataType(DataType.PhoneNumber)]
            public string TelephoneNumber { get; set; }

            [Display(Name = "Username")]
            [Required]
            [DataType(DataType.Text)]
            public string Username { get; set; }

        }

        public IActionResult OnGet(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            returnUrl = returnUrl ?? Url.Content("~/");
            if(Input.TypeOfAccount!= "WorkerAccount"&&Input.TypeOfAccount!= "EmployerAccount")
            {
                ModelState.AddModelError("Account type", "Invalid account type.");
            }
            if (ModelState.IsValid)
            {
                User user = new User { Email = Input.Email, UserName = Input.Username, PhoneNumber=Input.TelephoneNumber };
                if (Input.TypeOfAccount=="WorkerAccount")
                {
                    user.WorkerAccount = new WorkerAccount();
                }
                else if(Input.TypeOfAccount=="EmployerAccount")
                {
                    user.EmployerAccount = new EmployerAccount();
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("AddAccount");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        //private async void EnsureRolesCreated()
        //{
        //    var employerRole = new IdentityRole("Employer");
        //    var workerRole = new IdentityRole("Worker");
        //    var employerRoleExists =  await _roleManager.RoleExistsAsync("Employer");
        //    var workerRoleExists = await _roleManager.RoleExistsAsync("Worker");
        //    if (!employerRoleExists)
        //    {
        //        var result = await _roleManager.CreateAsync();
        //        if(result.Succe)
        //    }
        //}
    }
}
