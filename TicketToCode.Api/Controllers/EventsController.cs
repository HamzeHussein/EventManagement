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

        /// <summary>
        /// Hämtar alla evenemang
        /// </summary>
        /// <returns>Lista med alla evenemang</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        /// <summary>
        /// Hämtar ett specifikt evenemang baserat på ID
        /// </summary>
        /// <param name="id">Evenemangets ID</param>
        /// <returns>Det specifika evenemanget</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
                return NotFound();
            return eventItem;
        }

        /// <summary>
        /// Skapar ett nytt evenemang
        /// </summary>
        /// <param name="eventItem">Evenemangets information</param>
        /// <returns>Det skapade evenemanget</returns>
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event eventItem)
        {
            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvent), new { id = eventItem.Id }, eventItem);
        }

        /// <summary>
        /// Uppdaterar ett specifikt evenemang
        /// </summary>
        /// <param name="id">Evenemangets ID</param>
        /// <param name="eventItem">Evenemangets nya information</param>
        /// <returns>Ingen returnering om uppdateringen är lyckad</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event eventItem)
        {
            if (id != eventItem.Id)
                return BadRequest();

            _context.Entry(eventItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Tar bort ett evenemang baserat på ID
        /// </summary>
        /// <param name="id">Evenemangets ID</param>
        /// <returns>Ingen returnering om borttagning är lyckad</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
                return NotFound();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}

