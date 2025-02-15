using Api_1.Entity.user;
using Api_1.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_1.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(UserServiec userServiec)
    : ControllerBase
{
    private readonly UserServiec userServiec = userServiec;

    [HttpGet]
    public async Task<IActionResult> GetAllUser(CancellationToken cancellationToken)
    {
        var users = await userServiec.GetAllUser();
        return Ok(users);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] string id, CancellationToken cancellationToken)
    {
        var user = await userServiec.GetUserById(id);
        if (user is null)
            return NotFound();
        return Ok(user);
    }
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] AddUser user, CancellationToken cancellationToken)
    {
        var result = await userServiec.AddNewUser(user, cancellationToken);
        if (result is null)
            return BadRequest();
        else if (result.Id == "this role invalid")
            return BadRequest("this role invalid");
        else if (result.Id == "use defrent Email")
            return BadRequest("use defrent Email");
        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] AddUser user, CancellationToken cancellationToken)
    {
        var result = await userServiec.UpdateUser(id, user, cancellationToken);
        if (result is null)
            return BadRequest();
        else if (result.Id == "this role invalid")
            return BadRequest("this role invalid");
        else if (result.Id == "use defrent Email")
            return BadRequest("use defrent Email");
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await userServiec.DeleteUser(id, cancellationToken);
        if (!result)
            return NotFound();
        return Ok();
    }
    [HttpPut("/unlookUser")]
    public async Task<IActionResult> UnlookEmail(string id, CancellationToken cancellationToken)
    {
        var result = await userServiec.unlookUser(id, cancellationToken);
        if (!result)
            return NotFound();
        return Ok();
    }
}
