using Microsoft.AspNetCore.Authorization;

namespace Api_1.Entity.filters;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}