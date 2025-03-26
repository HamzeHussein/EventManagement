using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TicketToCode.Core.Models; // För att kunna använda Event-klassen

namespace TicketToCode.Client.Services
{
    public class EventService
    {
        private readonly HttpClient _httpClient;

        // Konstruktor för att ta emot HttpClient
        public EventService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Hämtar alla evenemang
        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<Event>>("api/events");
                return response ?? new List<Event>(); // Returnerar tom lista om inget svar
            }
            catch (Exception)
            {
                // Hantera eventuella fel och returnera en tom lista
                return new List<Event>();
            }
        }

        // Hämtar ett specifikt evenemang baserat på ID
        public async Task<Event> GetEventAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Event>($"api/events/{id}");
                return response ?? new Event(); // Returnerar ett nytt event om inget svar
            }
            catch (Exception)
            {
                // Hantera eventuella fel och returnera ett nytt Event
                return new Event();
            }
        }

        // Skapar ett nytt evenemang
        public async Task<bool> CreateEventAsync(Event newEvent)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/events", newEvent);
                return response.IsSuccessStatusCode; // Returnerar true om statuskoden är success
            }
            catch (Exception)
            {
                // Om något fel inträffar vid skapandet
                return false;
            }
        }

        // Tar bort ett evenemang baserat på ID
        public async Task<bool> DeleteEventAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/events/{id}");
                return response.IsSuccessStatusCode; // Returnerar true om statuskoden är success
            }
            catch (Exception)
            {
                // Om något fel inträffar vid borttagningen
                return false;
            }
        }

        // 🆕 Bokar en biljett till ett evenemang
        public async Task<bool> BookTicketAsync(int eventId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/bookings", new { EventId = eventId });
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 🆕 Hämtar alla bokningar för inloggad användare
        public async Task<List<Booking>> GetMyBookingsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Booking>>("api/bookings");
                return response ?? new List<Booking>();
            }
            catch
            {
                return new List<Booking>();
            }
        }

        // 🆕 Avbokar en bokning via ID
        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/bookings/{bookingId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}


