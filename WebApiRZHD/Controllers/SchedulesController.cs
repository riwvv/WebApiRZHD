using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRZHD.DTOs;
using WebApiRZHD.Models;

namespace WebApiRZHD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase {
    private readonly RZHDContext _context;

    public SchedulesController(RZHDContext rZHDContext) {
        _context = rZHDContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules() {
        var response = await _context.Schedules.ToListAsync();
        var responseDTO = new List<ScheduleDTO>();
        foreach (var item in response) {
            responseDTO.Add(new ScheduleDTO {
                Id = item.Id,
                Departure_station = item.Departure_stationNavigation.Name,
                Arrival_station = item.Arrival_stationNavigation.Name,
                Departure_date_time = item.Departure_date_time,
                Arrival_date_time = item.Arrival_date_time,
                Number_of_available_seats = item.Number_of_available_seats,
                Ticket_price = item.Ticket_price
            });
        }
        return Ok(responseDTO);
    }


}
