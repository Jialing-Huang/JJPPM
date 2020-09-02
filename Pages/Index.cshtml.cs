using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace JJPPM.Pages
{
  public class IndexModel : PageModel
  {
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(SignInManager<IdentityUser> signInManager, ILogger<IndexModel> logger)
    {
      _signInManager = signInManager;
      _logger = logger;
    }

    public IActionResult OnGet()
    {
      // JH, 2020-09-01, Added authentication
      if (_signInManager.IsSignedIn(User))
      {
        return LocalRedirect("/Projects");
      }
      return LocalRedirect("/Welcome");
    }
  }
}
