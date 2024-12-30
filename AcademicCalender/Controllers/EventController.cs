using AcademicCalender.Entity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    // Tüm Event'leri Getir
    [HttpGet]
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    // Belirli bir Event'i ID ile Getir
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventById(int id)
    {
        var eventItem = await _eventService.GetEventByIdAsync(id);
        if (eventItem == null)
        {
            return NotFound($"Event with ID {id} not found.");
        }
        return Ok(eventItem);
    }

    // Yeni Event Ekle
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto createEventDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newEvent = new Event
        {
            Title = createEventDto.Title,
            Description = createEventDto.Description,
            StartDate = createEventDto.StartDate,
            EndDate = createEventDto.EndDate,
            CalendarID = createEventDto.CalendarID
        };

        await _eventService.AddEventAsync(newEvent);
        return CreatedAtAction(nameof(GetEventById), new { id = newEvent.EventID }, newEvent);
    }


    // Event Güncelle
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event eventItem)
    {
        if (id != eventItem.EventID)
        {
            return BadRequest("Event ID mismatch.");
        }

        await _eventService.UpdateEventAsync(eventItem);
        return NoContent();
    }

    // Event Sil
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        await _eventService.DeleteEventAsync(id);
        return NoContent();
    }

    // Belirli bir Calendar ID'ye ait Event'leri Getir
    [HttpGet("calendar/{calendarId}")]
    public async Task<IActionResult> GetEventsByCalendarId(int calendarId)
    {
        var events = await _eventService.GetEventsByCalendarIdAsync(calendarId);
        if (!events.Any())
        {
            return NotFound($"No events found for calendar ID {calendarId}.");
        }
        return Ok(events);
    }
}
