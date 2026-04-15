using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRZHD.DTOs;
using WebApiRZHD.Models;

namespace WebApiRZHD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PassportDataController : Controller {
    private readonly RZHDContext _context;

    public PassportDataController(RZHDContext rZHDContext) {
        _context = rZHDContext;
    }

    [HttpGet("{login}")]
    public async Task<ActionResult<Passport_datum>> GetPassportData(string login) {
        var result = await _context.Passport_data.FirstOrDefaultAsync(x => x.Login == login);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Passport_datum>> CreatePassportData([FromBody] PassportDataDTO passportDataDTO) {
        var passports = await _context.Passport_data.ToListAsync();
        var newPassportData = new Passport_datum {
            Id = passports.Count > 0 ? passports.Max(x => x.Id) + 1 : 1,
            Login = passportDataDTO.Login,
            Passport_series = passportDataDTO.Passport_series,
            Passport_number = passportDataDTO.Passport_number,
            First_name = passportDataDTO.First_name,
            Last_name = passportDataDTO.Last_name,
            Middle_name = passportDataDTO.Middle_name
        };

        try {
            _context.Passport_data.Add(newPassportData);
            await _context.SaveChangesAsync();
            return Ok(newPassportData);
        }
        catch (Exception ex) {
            return BadRequest($"Ошибка: {ex.Message}");
        }
    }
}
