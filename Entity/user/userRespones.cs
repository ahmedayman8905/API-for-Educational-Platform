using System.ComponentModel.DataAnnotations;

namespace Api_1.Entity.user;

public class userRespones
{
    public string Id { get; set; }

    public string? FullName { get; set; }

    public string Email { get; set; }

    public bool? IsDelete { get; set; } 

    public List<string>? Roles { get; set; } = []; 

}
