using Api_1.Entity.Consts;
using Api_1.Entity.user;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Api_1.Repository;

public class UserServiec(
    EducationalPlatformContext educationalPlatformContext
    , UserManager<Student> userManager
    )
{
    private readonly UserManager<Student> userManager = userManager;

    public EducationalPlatformContext db { get; } = educationalPlatformContext;

    public async Task<List<userRespones>> GetAllUser(CancellationToken cancellationToken = default)
    {
        //var userRespones = await db.Users.Select(Task => new userRespones
        //{
        //    Id = Task.Id,
        //    FullName = Task.FullName!,
        //    Email = Task.Email,
        //    IsDelete = Task.IsDelete,
        //    Roles = db.UserRoles.Where(r => r.UserId == Task.Id).Select(r => r.RoleId).ToList()
        //}).ToListAsync();


        //foreach (var user in userRespones)
        //{
        //    if (user.Roles != null && user.Roles!.Count > 0)
        //    {
        //        var roles= await db.Roles.Where(r => user.Roles.Contains(r.Id)).ToListAsync();
        //        user.Roles = roles.Select(r => r.Name!).ToList();
        //    }
        //}
        // return userRespones;
        return await (from u in db.Users
               join ur in db.UserRoles
               on u.Id equals ur.UserId
               join r in db.Roles
               on ur.RoleId equals r.Id into roles
              // where !roles.Any(x => x.Name == DefaultRoles.Member)
               select new
               {
                   u.Id,
                   u.FullName,
                   u.Email,                  
                   u.IsDelete,
                   Roles = roles.Select(x => x.Name).ToList()
               }
               )
               .GroupBy(u => new { u.Id, u.FullName, u.Email, u.IsDelete })
               .Select(u => new userRespones 
               {


                   Id =  u.Key.Id,
                   FullName =  u.Key.FullName,
                   Email =  u.Key.Email,
                   IsDelete = u.Key.IsDelete,
                   Roles = u.SelectMany(x => x.Roles).ToList()
               })
              .ToListAsync(cancellationToken);
    }

    public async Task<userRespones>GetUserById(string id, CancellationToken cancellationToken = default)
    {
        var user = await db.Students.SingleOrDefaultAsync(i => i.Id == id , cancellationToken);
        if (user is null)
        {
            return null!;
        }
        //var userRespones = new userRespones
        //{
        //    Id = user.Id,
        //    FullName = user.FullName!,
        //    Email = user.Email,
        //    IsDelete = user.IsDelete,
        //    Roles = userManager.GetRolesAsync(user).Result.ToList()
        //};
        var userrespones = user.Adapt<userRespones>();
        userrespones.Roles = userManager.GetRolesAsync(user).Result.ToList();

        return userrespones;
    } 


    public async Task<userRespones> AddNewUser(AddUser newUser, CancellationToken cancellationToken = default)
    {
        var user = db.Students.SingleOrDefault(u => u.Email == newUser.Email);
        if (user != null)
        {
            return new userRespones { Id = "use defrent Email" };
        }

        var allRoles = await db.Roles.Select(i => i.Name).ToListAsync();
        if (newUser.Roles!.Except(allRoles).Any())
        {
            return new userRespones { Id = "this role invalid"};
        }
        var newUserEntity = newUser.Adapt<Student>();
        newUserEntity.UserName = newUser.Email;
        newUserEntity.EmailConfirmed = true;
        var result = await userManager.CreateAsync(newUserEntity, newUser.Password);
        if (result.Succeeded)
        {
            // add new roles for this user
            await userManager.AddToRolesAsync(newUserEntity, newUser.Roles!);
            var reslt = newUserEntity.Adapt<userRespones>();
            reslt.Roles = newUser.Roles;
            return reslt;

        }

        return null!;
    }

    public async Task<userRespones> UpdateUser( string id , AddUser newUser, CancellationToken cancellationToken = default)
    {
        var user = db.Students.SingleOrDefault(u => u.Email == newUser.Email && u.Id != id);
        if (user != null)
        {
            return new userRespones { Id = "use defrent Email" };
        }

        var allRoles = await db.Roles.Select(i => i.Name).ToListAsync();
        if (newUser.Roles!.Except(allRoles).Any())
        {
            return new userRespones { Id = "this role invalid" };
        }

        var userEntity = await db.Students.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (userEntity is null)
        {
            return null!;
        }
        userEntity = newUser.Adapt(userEntity);
        userEntity.UserName = newUser.Email;
        userEntity.EmailConfirmed = true;
        var result = await userManager.UpdateAsync(userEntity);
        if (result.Succeeded)
        {
            // remove old roles for this user
            var oldRoles = await userManager.GetRolesAsync(userEntity);
            await userManager.RemoveFromRolesAsync(userEntity, oldRoles);

            // add new roles for this user
            await userManager.AddToRolesAsync(userEntity, newUser.Roles!);
            var reslt = userEntity.Adapt<userRespones>();
            reslt.Roles = newUser.Roles;
            return reslt;
        }
        return null!;
    }

    public async Task<bool> DeleteUser(string id, CancellationToken cancellationToken = default)
    {
        var user = await db.Students.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (user is null)
        {
            return false;
        }
        user.IsDelete = true;
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> unlookUser(string id, CancellationToken cancellationToken = default)
    {
        var user = await db.Students.SingleOrDefaultAsync(u => u.Id == id
         && u.EmailConfirmed && u.IsDelete == false , cancellationToken);
        if (user is null)
        {
            return false;
        }
        user.LockoutEnd = DateTime.UtcNow.AddDays(-1);
        
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }


}
