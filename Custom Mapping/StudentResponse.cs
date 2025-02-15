namespace Api_1.Contract;

public class StudentResponse
{
 
    public string id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public string? Gender { get; set; }
    public string Token {  get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiretion { get; set; }

}
