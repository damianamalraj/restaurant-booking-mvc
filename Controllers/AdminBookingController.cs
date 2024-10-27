using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingMvc.Models;

namespace RestaurantBookingMvc.Controllers
{
  [Authorize(Roles = "Admin")]
  public class AdminBookingController : Controller
  {
    private readonly HttpClient _client;
    private readonly string baseUri = "http://localhost:5211";

    public AdminBookingController(HttpClient client)
    {
      _client = client;
    }

    public async Task<IActionResult> Index()
    {
      var response = await _client.GetAsync($"{baseUri}/api/Bookings");

      if (response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        var bookings = JsonConvert.DeserializeObject<List<Booking>>(json);

        return View(bookings);

      }

      return View();
    }

    public async Task<IActionResult> Create()
    {
      var avalableTables = await _client.GetAsync($"{baseUri}/api/Tables");

      if (avalableTables.IsSuccessStatusCode)
      {
        var json = await avalableTables.Content.ReadAsStringAsync();
        var tables = JsonConvert.DeserializeObject<List<Table>>(json);

        ViewBag.Tables = tables;
      }

      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Booking booking)
    {
      if (!ModelState.IsValid)
      {
        var avalableTables = await _client.GetAsync($"{baseUri}/api/Tables");

        if (avalableTables.IsSuccessStatusCode)
        {
          var data = await avalableTables.Content.ReadAsStringAsync();
          var tables = JsonConvert.DeserializeObject<List<Table>>(data);

          ViewBag.Tables = tables;
        }

        return View(booking);
      }

      var json = JsonConvert.SerializeObject(booking);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await _client.PostAsync($"{baseUri}/api/Bookings", content);

      if (response.IsSuccessStatusCode)
      {
        return RedirectToAction("Index", "AdminBooking");
      }

      var errorMessage = await response.Content.ReadAsStringAsync();
      ModelState.AddModelError(string.Empty, $"Error: {errorMessage}");
      return View(booking);
    }

    public async Task<IActionResult> Edit(int id)
    {
      var response = await _client.GetAsync($"{baseUri}/api/Bookings/{id}");
      var avalableTables = await _client.GetAsync($"{baseUri}/api/Tables");

      if (response.IsSuccessStatusCode && avalableTables.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        var booking = JsonConvert.DeserializeObject<Booking>(json);

        var jsonTables = await avalableTables.Content.ReadAsStringAsync();
        var tables = JsonConvert.DeserializeObject<List<Table>>(jsonTables);

        ViewBag.Tables = tables;

        return View(booking);
      }

      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Booking booking)
    {
      if (!ModelState.IsValid)
      {
        var avalableTables = await _client.GetAsync($"{baseUri}/api/Tables");

        if (avalableTables.IsSuccessStatusCode)
        {
          var data = await avalableTables.Content.ReadAsStringAsync();
          var tables = JsonConvert.DeserializeObject<List<Table>>(data);

          ViewBag.Tables = tables;
        }

        return View(booking);
      }

      var json = JsonConvert.SerializeObject(booking);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await _client.PutAsync($"{baseUri}/api/Bookings/{booking.Id}", content);

      if (response.IsSuccessStatusCode)
      {
        return RedirectToAction("Index", "AdminBooking");
      }

      var errorMessage = await response.Content.ReadAsStringAsync();
      ModelState.AddModelError(string.Empty, $"Error: {errorMessage}");
      return View(booking);
    }

    public async Task<IActionResult> Delete(int id)
    {
      var response = await _client.DeleteAsync($"{baseUri}/api/Bookings/{id}");

      if (response.IsSuccessStatusCode)
      {
        return RedirectToAction("Index", "AdminBooking");
      }

      return View();
    }
  }
}