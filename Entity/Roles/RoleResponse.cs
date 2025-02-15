using System.ComponentModel.DataAnnotations;

namespace Api_1.Entity.Roles;

public class RoleResponse
{
    
    public string Id {get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
}
