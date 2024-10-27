using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class AccountController : Controller
{
  private const string AdminUsername = "Admin";
  private const string AdminPassword = "admin123";

  public IActionResult Login()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Login(string username, string password)
  {
    if (username == AdminUsername && password == AdminPassword)
    {
      var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

      var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      var authProperties = new AuthenticationProperties
      {
        IsPersistent = false
      };

      await HttpContext.SignInAsync(
          CookieAuthenticationDefaults.AuthenticationScheme,
          new ClaimsPrincipal(claimsIdentity),
          authProperties);

      return RedirectToAction("Index", "Home");
    }

    ModelState.AddModelError("", "Invalid username or password");
    return View();
  }

  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Index", "Home");
  }
}