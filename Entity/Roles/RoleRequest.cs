
namespace Api_1.Entity.Roles;


public record RoleRequest(
    string Name,
    List<string> Permissions
);