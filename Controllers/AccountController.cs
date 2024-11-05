using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingMvc.Models;
using System.Security.Claims;
using System.Text;

public class AccountController : Controller
{
  private readonly HttpClient _client;
  private readonly IConfiguration _configuration;
  private readonly string? baseUri;

  public AccountController(HttpClient client, IConfiguration configuration)
  {
    _client = client;
    _configuration = configuration;
    baseUri = _configuration["BaseUri"];
  }

  public IActionResult Login()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Login(Login login)
  {
    if (!ModelState.IsValid)
    {
      return View(login);
    }

    var json = JsonConvert.SerializeObject(login);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await _client.PostAsync($"{baseUri}/api/Auth/Login", content);

    if (!response.IsSuccessStatusCode)
    {
      ModelState.AddModelError(string.Empty, "Invalid login attempt.");
      return View(login);
    }

    var token = await response.Content.ReadAsStringAsync();

    var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Email, login.Email),
        new Claim("token", token)
      };

    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

    return RedirectToAction("Index", "Home");
  }

  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    return RedirectToAction("Index", "Home");
  }
}