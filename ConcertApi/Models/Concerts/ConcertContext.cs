using Microsoft.EntityFrameworkCore;

namespace ConcertApi.Models.Concerts;

public class ConcertContext : DbContext
{
  public ConcertContext(DbContextOptions<ConcertContext> options)
    : base(options)
  {
  }
  public DbSet<Concert> Concerts { get; set; } = null!;
  public DbSet<Band> Bands { get; set; } = null!;
  public DbSet<Venue> Venues { get; set; } = null!;
}