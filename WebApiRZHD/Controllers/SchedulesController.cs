using Azure;
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

    [HttpPost]
    public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedulesByInfo([FromBody] ScheduleDTO scheduleDTO, [FromQuery] DateTime? returnDate = null) {
        var ds = await _context.Stations.FirstOrDefaultAsync(x => x.Name == scheduleDTO.Departure_station);
        var ass = await _context.Stations.FirstOrDefaultAsync(x => x.Name == scheduleDTO.Arrival_station);
        if (ds == null || ass == null)
            return BadRequest();
        var result = await _context.Schedules.Where(x => x.Number_of_available_seats > 0 && x.Departure_station == ds.Id && x.Arrival_station == ass.Id && x.Departure_date_time.Date == scheduleDTO.Departure_date_time.Date).ToListAsync();
        var resultDTO = new List<ScheduleDTO>();
        if (returnDate == null) {
            foreach (var item in result) {
                resultDTO.Add(new ScheduleDTO {
                    Id = item.Id,
                    Departure_station = item.Departure_stationNavigation.Name,
                    Arrival_station = item.Arrival_stationNavigation.Name,
                    Number_of_available_seats = item.Number_of_available_seats,
                    Ticket_price = item.Ticket_price,
                    Departure_date_time = item.Departure_date_time,
                    Arrival_date_time = item.Arrival_date_time
                });
            }
            return Ok(resultDTO);
        }
        result.AddRange(await _context.Schedules.Where(x => x.Number_of_available_seats > 0 && x.Departure_date_time.Date == returnDate.Value.Date && x.Departure_station == ass.Id && x.Arrival_station == ds.Id).ToListAsync());
        foreach (var item in result) {
            resultDTO.Add(new ScheduleDTO {
                Id = item.Id,
                Departure_station = item.Departure_stationNavigation.Name,
                Arrival_station = item.Arrival_stationNavigation.Name,
                Number_of_available_seats = item.Number_of_available_seats,
                Ticket_price = item.Ticket_price,
                Departure_date_time = item.Departure_date_time,
                Arrival_date_time = item.Arrival_date_time
            });
        }
        return Ok(resultDTO);
    }

    [HttpPost("new")]
    public async Task<ActionResult<ScheduleDTO>> CreateSchedule([FromBody] ScheduleDTO schedule) {
        int newId = (await _context.Schedules.ToListAsync()).Max(x => x.Id);
        var newSchedule = new Schedule {
            Id = newId + 1,
            Departure_station = (await _context.Stations.FirstOrDefaultAsync(x => x.Name == schedule.Departure_station))!.Id,
            Arrival_station = (await _context.Stations.FirstOrDefaultAsync(x => x.Name == schedule.Arrival_station))!.Id,
            Departure_date_time = schedule.Departure_date_time,
            Arrival_date_time = (DateTime)schedule.Arrival_date_time!,
            Number_of_available_seats = (int)schedule.Number_of_available_seats!,
            Ticket_price = (decimal)schedule.Ticket_price!
        };
        try {
            _context.Add(newSchedule);
            await _context.SaveChangesAsync();
            return StatusCode(204, schedule);
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}
