using Microsoft.AspNetCore.Authorization;
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

    public async Task<IActionResult> Index(int startPrice = 0, int endPrice = 0)
    {
      var response = await _client.GetAsync($"{baseUri}/api/MenuItems");

      if (response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        var menuItems = JsonConvert.DeserializeObject<List<MenuItem>>(json);

        if (startPrice > 0 && endPrice > 0)
        {
          menuItems = menuItems?.Where(x => x.Price >= startPrice && x.Price <= endPrice).ToList();
        }

        return View(menuItems);
      }

      return View();

    }

  }
}