﻿using Api_1.Entity.Consts;
using Microsoft.AspNetCore.Authorization;
namespace Api_1.Entity.filters;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        //var user = context.User.Identity;

        //if(user is null || !user.IsAuthenticated)
        //    return;

        //var hasPermission = context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type);

        //if(!hasPermission) 
        //    return;

        if (context.User.Identity is not { IsAuthenticated: true } ||
            !context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type))
            return;

        context.Succeed(requirement);
        return;
    }
}