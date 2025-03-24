using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketToCode.Core.Data;
using TicketToCode.Core.Models;

namespace TicketToCode.Api.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventManagementDbContext _context;

        public EventsController(EventManagementDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            try
            {
                var eventItem = await _context.Events.FindAsync(id);
                if (eventItem == null)
                    return NotFound();  // Returnera 404 om inte hittat
                return Ok(eventItem);  // Returnera evenemanget
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");  // Hantera fel
            }
        }

       
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event eventItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Validering av modelldatat

            try
            {
                _context.Events.Add(eventItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEvent), new { id = eventItem.Id }, eventItem);  // Returnera 201 status och det skapade evenemanget
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");  // Hantera eventuella fel vid sparande
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event eventItem)
        {
            if (id != eventItem.Id)
                return BadRequest("Event ID mismatch");  // Fel om ID:n inte matchar

            _context.Entry(eventItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                    return NotFound();  // Returnera 404 om inte eventet finns
                else
                    throw;  // Hantera alla andra undantag
            }

            return NoContent();  // Retur 204 när uppdateringen är klar
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
                return NotFound();  // Returnera 404 om eventet inte finns

            try
            {
                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");  // Hantera fel vid borttagning
            }

            return NoContent();  // Returnera 204 när borttagning är slutförd
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);  // Kolla om eventet finns i databasen
        }
    }
}

