using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingMvc.Models;

namespace RestaurantBookingMvc.Controllers
{
  [Authorize(Roles = "Admin")]
  public class AdminMenuController : Controller
  {
    private readonly HttpClient _client;
    private readonly string baseUri = "http://localhost:5211";

    public AdminMenuController(HttpClient client)
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

    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(MenuItem menuItem)
    {
      if (!ModelState.IsValid)
      {
        return View(menuItem);
      }

      var json = JsonConvert.SerializeObject(menuItem);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await _client.PostAsync($"{baseUri}/api/MenuItems", content);

      if (response.IsSuccessStatusCode)
      {
        return RedirectToAction("Index", "AdminMenu");
      }

      return View();
    }

    public async Task<IActionResult> Edit(int id)
    {
      var response = await _client.GetAsync($"{baseUri}/api/MenuItems/{id}");

      if (response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        var menuItem = JsonConvert.DeserializeObject<MenuItem>(json);

        return View(menuItem);

      }

      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(MenuItem menuItem)
    {
      if (!ModelState.IsValid)
      {
        return View(menuItem);
      }

      var json = JsonConvert.SerializeObject(menuItem);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await _client.PutAsync($"{baseUri}/api/MenuItems/{menuItem.Id}", content);

      if (response.IsSuccessStatusCode)
      {
        return RedirectToAction("Index", "AdminMenu");
      }

      return View();
    }

    public async Task<IActionResult> Delete(int id)
    {
      var response = await _client.DeleteAsync($"{baseUri}/api/MenuItems/{id}");

      if (response.IsSuccessStatusCode)
      {
        return RedirectToAction("Index", "AdminMenu");
      }

      return View();
    }

  }
}