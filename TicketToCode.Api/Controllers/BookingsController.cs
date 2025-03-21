using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketToCode.Core.Data;
using TicketToCode.Core.Models;

namespace TicketToCode.Api.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly EventManagementDbContext _context;

        public BookingsController(EventManagementDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hämtar alla bokningar
        /// </summary>
        /// <returns>Lista med alla bokningar</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            try
            {
                var bookings = await _context.Bookings.ToListAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Hämtar en specifik bokning
        /// </summary>
        /// <param name="id">Bokningens ID</param>
        /// <returns>En bokning</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                    return NotFound();
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Skapar en ny bokning
        /// </summary>
        /// <param name="booking">Bokningsinformation</param>
        /// <returns>Den skapade bokningen</returns>
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Tar bort en bokning baserat på ID
        /// </summary>
        /// <param name="id">Bokningens ID</param>
        /// <returns>Resultat av borttagning</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                    return NotFound();

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
