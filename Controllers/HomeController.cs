using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingMvc.Models;

namespace RestaurantBookingMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _client;
    private readonly string baseUri = "http://localhost:5211";

    public HomeController(ILogger<HomeController> logger, HttpClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task<IActionResult> Index()
    {
        var response = await _client.GetAsync($"{baseUri}/api/MenuItems");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var menuItems = JsonConvert.DeserializeObject<List<MenuItem>>(json);

            var popularItems = menuItems?.Where(x => x.IsPopular).ToList();

            return View(popularItems);
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
