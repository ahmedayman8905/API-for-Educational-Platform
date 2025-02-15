using System.ComponentModel.DataAnnotations;

namespace Api_1.Entity.user;

public class AddUser
{
    
    public string? FullName { get; set; }

    public string Email { get; set; }

    public string? Phone { get; set; }
 
    public string? Gender { get; set; }

    public string Password { get; set; }

    public List<string>? Roles { get; set; } = []; 

}
