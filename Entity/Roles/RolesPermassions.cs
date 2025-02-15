using Api_1.Entity.Consts;

namespace Api_1.Entity.Roles;

public class RolesPermassions
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public List<string> permissions { get; set; } = [];
}
