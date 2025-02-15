using Microsoft.AspNetCore.Authorization;

namespace Api_1.Entity.filters;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{
}