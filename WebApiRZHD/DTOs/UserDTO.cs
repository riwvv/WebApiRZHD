namespace WebApiRZHD.DTOs; 
public class UserDTO {
    public string Login { get; set; }

    public string Password { get; set; }

    public string? Role { get; set; } = "user";
    public bool? IsPassportData { get; set; } = false;
}
