namespace WebApiRZHD.DTOs;

public class ScheduleDTO {
    public int Id { get; set; }

    public string Departure_station { get; set; }

    public string Arrival_station { get; set; }

    public DateTime Departure_date_time { get; set; }

    public DateTime Arrival_date_time { get; set; }

    public int Number_of_available_seats { get; set; }

    public decimal Ticket_price { get; set; }
}
