using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRZHD.Models;

namespace WebApiRZHD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StationsController : Controller {
    private readonly RZHDContext _context;

    public StationsController(RZHDContext rZHDContext) {
        _context = rZHDContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Station>>> GetStations() => Ok(await _context.Stations.ToListAsync());
}
