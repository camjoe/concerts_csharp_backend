using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcertApi.Models.Concerts;

namespace ConcertApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertsController : ControllerBase
    {
        private readonly ConcertContext _context;

        public ConcertsController(ConcertContext context)
        {
            _context = context;
        }

        // GET: api/Concerts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Concert>>> GetConcerts()
        {
            var concerts = await _context.Concerts
                .Include(concert => concert.Venue)
                .Include(concert => concert.HeadlinerBand)
                .ToListAsync();
                //.Include(concert => concert.SupportingBands)

            return concerts;
        }

        // GET: api/Concerts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Concert>> GetConcert(int id)
        {
            var concert = await _context.Concerts.FindAsync(id);

            if (concert == null)
            {
                return NotFound();
            }

            concert.Venue = ExpandVenue(concert.VenueId);
            concert.HeadlinerBand = ExpandBand(concert.HeadlinerBandID);
            if (concert.SupportingBandsIds != null) 
            {
                concert.SupportingBands = ExpandSupportingBands(concert.SupportingBandsIds);
            }
            return concert;
        }

        // PUT: api/Concerts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConcert(int id, Concert concert)
        {
            if (id != concert.Id)
            {
                return BadRequest();
            }

            _context.Entry(concert).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConcertExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Concerts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Concert>> PostConcert(Concert concert)
        {
            _context.Concerts.Add(concert);
            await _context.SaveChangesAsync();

            concert.Venue = ExpandVenue(concert.VenueId);
            concert.HeadlinerBand = ExpandBand(concert.HeadlinerBandID);
            if (concert.SupportingBandsIds != null) 
            {
                concert.SupportingBands = ExpandSupportingBands(concert.SupportingBandsIds);
            }

            //var listWithTasks = _context.Concerts.Include(x => x.Venues).Single(x => x.Id == task.ListId);
            return CreatedAtAction(nameof(GetConcert), new { id = concert.Id }, concert);
        }

        // DELETE: api/Concerts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcert(int id)
        {
            var concert = await _context.Concerts.FindAsync(id);
            if (concert == null)
            {
                return NotFound();
            }

            _context.Concerts.Remove(concert);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConcertExists(int id)
        {
            return _context.Concerts.Any(e => e.Id == id);
        }

        private Venue ExpandVenue(int id)
        {
            return _context.Venues.Single(venue => venue.Id == id);
        }

        private Band ExpandBand(int id)
        {
            return _context.Bands.Single(band => band.Id == id);
        }

        private List<Band> ExpandSupportingBands(int[] ids)
        {
            return [.. _context.Bands.Where(band => ids.Contains(band.Id))];
        }
        
    }
}
