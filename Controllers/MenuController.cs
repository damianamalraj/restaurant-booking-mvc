using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingMvc.Models;

namespace RestaurantBookingMvc.Controllers
{
  public class MenuController : Controller
  {
    private readonly HttpClient _client;
    private readonly string baseUri = "http://localhost:5211";

    public MenuController(HttpClient client)
    {
      _client = client;
    }

    public async Task<IActionResult> Index()
    {
      var response = await _client.GetAsync($"{baseUri}/api/MenuItems");

      if (response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        var menuItems = JsonConvert.DeserializeObject<List<MenuItem>>(json);

        return View(menuItems);

      }

      return View();

    }
  }
}