using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingMvc.Models
{
  public class MenuItem
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(1, 1000, ErrorMessage = "Price must be between 0 and 1000")]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }
    public bool IsPopular { get; set; }
  }
}