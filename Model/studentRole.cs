using Microsoft.AspNetCore.Identity;

namespace Api_1.Model;

public class UserRole : IdentityRole
{
    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}
