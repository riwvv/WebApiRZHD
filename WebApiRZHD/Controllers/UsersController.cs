using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRZHD.Models;

namespace WebApiRZHD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
    private readonly RZHDContext _context;

    public UsersController(RZHDContext rZHDContext) {
        _context = rZHDContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers() => Ok(await _context.Users.ToListAsync());

    [HttpGet("{login}")]
    public async Task<ActionResult<User>> GetUserByLogin(string login) {
        var result = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<User>> RegistrationUser([FromBody] User user) {
        var newUser = new User {
            Login = user.Login,
            Password = user.Password,
            Role = "user"
        };
        try {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(newUser);
        }
        catch (Exception ex) {
            return BadRequest($"Не удалось зарегистрировать пользователя: {ex.Message}");
        }
    }
}
