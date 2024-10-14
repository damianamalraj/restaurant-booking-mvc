namespace RestaurantBookingMvc.Models
{
  public class MenuItem
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsPopular { get; set; }
    public decimal Price { get; set; }
  }
}