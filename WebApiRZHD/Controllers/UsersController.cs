using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRZHD.DTOs;
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
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers() {
        var result = await _context.Users.ToListAsync();
        var resultDTO = new List<UserDTO>();
        foreach (var item in result) {
            resultDTO.Add(new UserDTO {
                Login = item.Login,
                Password = item.Password,
                Role = item.Role,
                IsPassportData = item.Passport_data.Any()
            });
        }
        return Ok(resultDTO);
    }

    [HttpGet("{login}")]
    public async Task<ActionResult<UserDTO>> GetUserByLogin(string login) {
        var result = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
        if (result == null)
            return NotFound();
        var resultDTO = new UserDTO {
            Login = login,
            Password = result.Password,
            Role = result.Role,
            IsPassportData = result.Passport_data.Any()
        };
        return Ok(resultDTO);
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> RegistrationUser([FromBody] UserDTO user) {
        var newUser = new UserDTO {
            Login = user.Login,
            Password = user.Password
        };
        try {
            _context.Users.Add(new User { Login = newUser.Login, Password = newUser.Password, Role = newUser.Role });
            await _context.SaveChangesAsync();
            return Ok(newUser);
        }
        catch (Exception ex) {
            return BadRequest($"Не удалось зарегистрировать пользователя: {ex.Message}");
        }
    }
}
