using System.ComponentModel.DataAnnotations.Schema;

namespace ConcertApi.Models.Concerts;

public class Concert
{
  public int Id { get; set; }
  [ForeignKey("Band")]
  public int HeadlinerBandID { get; set; }
  public virtual Band? HeadlinerBand { get; set; }
  public int[]? SupportingBandsIds { get; set; }
  public virtual List<Band>? SupportingBands { get; set; }
  public DateTime Date { get; set; }
  public decimal TicketPrice { get; set; }
  public int VenueId { get; set; }
  public virtual Venue? Venue { get; set; }
}