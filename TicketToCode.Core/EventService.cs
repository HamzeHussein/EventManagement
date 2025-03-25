using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TicketToCode.Core.Models; // Make sure this namespace matches your project

namespace TicketToCode.Client.Services
{
    public class EventService : IEventService // Consider implementing an interface
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EventService> _logger; // Added logger

        public EventService(HttpClient httpClient, ILogger<EventService> logger = null)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/events");

                if (!response.IsSuccessStatusCode)
                {
                    _logger?.LogWarning("Failed to get events. Status code: {StatusCode}", response.StatusCode);
                    return Enumerable.Empty<Event>();
                }

                var content = await response.Content.ReadFromJsonAsync<IEnumerable<Event>>();
                return content ?? Enumerable.Empty<Event>();
            }
            catch (HttpRequestException httpEx)
            {
                _logger?.LogError(httpEx, "HTTP error while fetching events");
                return Enumerable.Empty<Event>();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error while fetching events");
                return Enumerable.Empty<Event>();
            }
        }

        public async Task<Event> GetEventAsync(int id)
        {
            if (id <= 0)
            {
                _logger?.LogWarning("Invalid event ID: {EventId}", id);
                return null;
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/events/{id}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger?.LogWarning("Event with ID {EventId} not found", id);
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger?.LogWarning("Failed to get event. Status code: {StatusCode}", response.StatusCode);
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<Event>();
            }
            catch (HttpRequestException httpEx)
            {
                _logger?.LogError(httpEx, "HTTP error while fetching event with ID {EventId}", id);
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error while fetching event with ID {EventId}", id);
                return null;
            }
        }

        public async Task<(bool Success, Event CreatedEvent, string ErrorMessage)> CreateEventAsync(Event newEvent)
        {
            if (newEvent == null)
            {
                _logger?.LogWarning("Attempted to create null event");
                return (false, null, "Event cannot be null");
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/events", newEvent);

                if (response.IsSuccessStatusCode)
                {
                    var createdEvent = await response.Content.ReadFromJsonAsync<Event>();
                    return (true, createdEvent, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger?.LogWarning("Failed to create event. Status: {StatusCode}, Error: {Error}",
                    response.StatusCode, errorContent);

                return (false, null, $"Error: {response.StatusCode} - {errorContent}");
            }
            catch (HttpRequestException httpEx)
            {
                _logger?.LogError(httpEx, "HTTP error while creating event");
                return (false, null, httpEx.Message);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error while creating event");
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteEventAsync(int id)
        {
            if (id <= 0)
            {
                _logger?.LogWarning("Invalid event ID for deletion: {EventId}", id);
                return (false, "Invalid event ID");
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"api/events/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger?.LogWarning("Failed to delete event. Status: {StatusCode}, Error: {Error}",
                    response.StatusCode, errorContent);

                return (false, $"Error: {response.StatusCode} - {errorContent}");
            }
            catch (HttpRequestException httpEx)
            {
                _logger?.LogError(httpEx, "HTTP error while deleting event with ID {EventId}", id);
                return (false, httpEx.Message);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error while deleting event with ID {EventId}", id);
                return (false, ex.Message);
            }
        }
    }

    // Consider adding this interface for better testability and DI
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetEventsAsync();
        Task<Event> GetEventAsync(int id);
        Task<(bool Success, Event CreatedEvent, string ErrorMessage)> CreateEventAsync(Event newEvent);
        Task<(bool Success, string ErrorMessage)> DeleteEventAsync(int id);
    }
}
