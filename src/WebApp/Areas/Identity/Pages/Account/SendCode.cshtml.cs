using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core.Entities;

namespace WebApp.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class SendCodeModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<SendCodeModel> _logger;
    private readonly IEmailSender _emailSender;
    private readonly string _appName;

    public SendCodeModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<SendCodeModel> logger, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _appName = configuration["AppName"];
    }

    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Send Login Code Via")]
        public string Provider { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
    {
        // Ensure the user has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
        {
            throw new InvalidOperationException($"Unable to load two-factor authentication user.");
        }

        var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
        List<SelectListItem> factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        ViewData["FactorOptions"] = factorOptions;

        ReturnUrl = returnUrl;
        RememberMe = rememberMe;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        returnUrl = returnUrl ?? Url.Content("~/");

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            throw new InvalidOperationException($"Unable to load two-factor authentication user.");
        }

        // Generate the token and send it
        var code = await _userManager.GenerateTwoFactorTokenAsync(user, Input.Provider);
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new InvalidOperationException($"Unable to generate login code for two factor authentication via {Input.Provider}");
        }

        var message = $"Hi, Your security code to login into {_appName} is " + code;
        if (Input.Provider == "Email")
        {
            await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), $"Security Code to login into {_appName}", message);
        }

        return RedirectToPage("./VerifyCode", new { Provider = Input.Provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
    }
}
