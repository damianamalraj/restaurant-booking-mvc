using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingMvc.Models
{
  public class Booking
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "Table number is required")]
    [Range(1, 100, ErrorMessage = "Table number must be between 1 and 100")]
    public int TableNumber { get; set; }

    [Required(ErrorMessage = "Customer name is required")]
    public string? CustomerName { get; set; }

    [Required(ErrorMessage = "Customer email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? CustomerEmail { get; set; }

    [Required(ErrorMessage = "End booking date and time is required")]
    public int NumberOfPeople { get; set; }

    [Required(ErrorMessage = "Start booking date and time is required")]
    public DateTime StartBookingDateTime { get; set; }
  }
}