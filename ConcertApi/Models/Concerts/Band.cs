namespace ConcertApi.Models.Concerts;

public class Band
{
  public int Id { get; set; }
  public string? Name { get; set; } 
  //[System.ComponentModel.Bindable(true)]
  public string? ImageUrl { get; set; }
}