using Api_1.Entity.Consts;
using Api_1.Entity.Roles;
using Api_1.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_1.Controllers;
[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = DefaultRoles.Admin , AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RolesController(RolesServices rolesServices)
    : ControllerBase
{
    public RolesServices RolesServices { get; } = rolesServices;

    [HttpGet]
    public async Task<IActionResult> GetRoles([FromQuery] bool? disActive = false)
    {
        var result = await RolesServices.GetRoles(disActive);
        return Ok(result);
    }
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetRoleById([FromRoute] string Id, CancellationToken cancellationToken)
    {
        var result = await RolesServices.GetRoleById(Id, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
        //return Conflict("dd");
    }
    [HttpPost]
    public async Task<IActionResult> AddRolePermission(RoleRequest request , CancellationToken cancellationToken)
    {
        var result = await RolesServices.AddAsync(request, cancellationToken);
        if (result is null)
        {
            return BadRequest();
        }
        else if (result.Id == "invalid permissions")
        {
            return Conflict("invalid permissions");
        }
        else if (result.Id == "use diffrent Role name ")
        {
            return Conflict("use diffrent Role name ");
        }

        return Ok(result);
        
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRolePermission([FromRoute] string id , RoleRequest request, CancellationToken cancellationToken)
    {
        var result = await RolesServices.UpdateAsync( id , request, cancellationToken);
        if (result is null)
        {
            return BadRequest();
        }
        else if (result.Id == "invalid permissions")
        {
            return Conflict("invalid permissions");
        }
        else if (result.Id == "use diffrent Role name ")
        {
            return Conflict("use diffrent Role name ");
        }
        else if (result.Id == "invalid Role")
        {
            return NotFound("invalid Role");
        }

        return Ok(result);

    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRolePermission([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await RolesServices.DeleteAsync(id, cancellationToken);
        if (result)
            return Ok();
        return BadRequest();
    }

}
