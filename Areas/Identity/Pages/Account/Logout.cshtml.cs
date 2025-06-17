// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using mmrcis.Models;
using mmrcis.Services;
using Microsoft.EntityFrameworkCore;

namespace mmrcis.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IAuditService _auditService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, IAuditService auditService, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _auditService = auditService;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var currentUser = await _userManager.Users
                                                .Include(u => u.Person) 
                                                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            string currentUserName = currentUser.Person.FullName;
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            string currentAction = "Logout";
            string currentController = "Account";
            string currentParameters = "";
            string currentIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            string currentUserAgent = Request.Headers["User-Agent"].ToString();
            await _auditService.LogActionAsync(
                    currentUserName,
                    currentAction,
                    currentController,
                    currentParameters,
                    currentIpAddress,
                    currentUserAgent
                    );
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
