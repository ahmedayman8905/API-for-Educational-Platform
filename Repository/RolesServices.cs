using Api_1.Entity.Consts;
using Api_1.Entity.Roles;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api_1.Repository;

public class RolesServices(EducationalPlatformContext _db , RoleManager<UserRole> roleManager)
{
    public EducationalPlatformContext Db { get; } = _db;
    public RoleManager<UserRole> RoleManager { get; } = roleManager;

    public async Task<List<RoleResponse>> GetRoles(bool? DesActive = false , CancellationToken cancellationToken = default)
    {
        
        //var roles = await Db.Roles.ProjectToType<RoleResponse>().ToListAsync();
        
        
        //if (DesActive.HasValue == true)
        //{
        //    return  roles;
        //}
        //else
        //{
        //    return roles.Where(r => r.IsDeleted == false).ToList();
        //}
        var roles = await Db.Roles.Where(r => r.IsDeleted == DesActive).ProjectToType<RoleResponse>().ToListAsync();
        return roles;


           //return   await _roleManager.Roles
           // .Where(x => !x.IsDefault && (!x.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))
           // .ProjectToType<RoleResponse>()
           // .ToListAsync(cancellationToken);
    }

    public async Task<RolesPermassions> GetRoleById(string id, CancellationToken cancellationToken = default)
    {
        var role = await Db.Roles.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (role is null)
        {
            return null;
        }
        var rolePermassions = new RolesPermassions
        {
            Name = role.Name!,
            Id = role.Id,
            IsDeleted = role.IsDeleted,
            permissions = RoleManager.GetClaimsAsync(role).Result
            .Select(c => c.Value).ToList()
        };

         return rolePermassions;
    }

    public async Task<RolesPermassions> AddAsync(RoleRequest request , CancellationToken cancellationToken = default)
    {
        var roleIsExists = await roleManager.RoleExistsAsync(request.Name);
       
        if (roleIsExists)
            return new RolesPermassions { Id = "use diffrent Role name " }; ;

        var allowedPermissions = Permissions.GetAllPermissions();

        if (request.Permissions.Except(allowedPermissions).Any())
            return new RolesPermassions { Id = "invalid permissions"};
        // add Role
        var role = new UserRole
        {
            Name = request.Name,
            NormalizedName = request.Name.ToUpper(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        var result = await roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            // add permissions
            var permissions = request.Permissions
                .Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = role.Id
                });

            await Db.AddRangeAsync(permissions, cancellationToken);
            await Db.SaveChangesAsync(cancellationToken);

            var response = new RolesPermassions
            {
                permissions = request.Permissions,
                IsDeleted = role.IsDeleted,
                Id = role.Id,
                Name = request.Name,
            };

            return response;
        }

        var error = result.Errors.First();

        return null!;
    }

    public async Task<RolesPermassions> UpdateAsync( string id ,RoleRequest request, CancellationToken cancellationToken = default)
    {
        var roleIsExists = await roleManager.Roles.AnyAsync(x => x.Name == request.Name && x.Id != id);

        if (roleIsExists)
            return new RolesPermassions { Id = "use diffrent Role name " };

        var allowedPermissions = Permissions.GetAllPermissions();

        if (request.Permissions.Except(allowedPermissions).Any())
            return new RolesPermassions { Id = "invalid permissions" };

        var role = await roleManager.FindByIdAsync(id);
        if (role == null)
            return new RolesPermassions { Id = "invalid Role" };
        // update Role
        role.Name = request.Name;
        var result = await roleManager.UpdateAsync(role);

        if (result.Succeeded)
        {
            // add && update permissions

            var currentPermissions = await Db.RoleClaims
              .Where(x => x.RoleId == id && x.ClaimType == Permissions.Type)
              .Select(x => x.ClaimValue)
              .ToListAsync();

            var newPermissions = request.Permissions.Except(currentPermissions)
                .Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = role.Id
                });

            var removedPermissions = currentPermissions.Except(request.Permissions).ToList().Select(i => i);

            var oldPermissions = await Db.RoleClaims
            .Where(x => x.RoleId == id)
            .ToListAsync();

            oldPermissions = oldPermissions.Where(x => removedPermissions.Contains(x.ClaimValue)).ToList();

            Db.RoleClaims.RemoveRange(oldPermissions);

            // await Task.Run(() => Db.RemoveRange(removedPermissions));
            await Db.AddRangeAsync(newPermissions , cancellationToken);
            await Db.SaveChangesAsync(cancellationToken);
            var response = new RolesPermassions
            {
                permissions = request.Permissions,
                IsDeleted = role.IsDeleted,
                Id = role.Id,
                Name = request.Name,
            };

            return response;
        }

        var error = result.Errors.First();

        return null!;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role == null)
            return false;
        role.IsDeleted = true;  
        await RoleManager.UpdateAsync(role);
        return true;
    }

}
