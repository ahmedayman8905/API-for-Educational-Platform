
using Api_1.Contract;
using Api_1.Entity;
using Api_1.Entity.Consts;
using Api_1.Entity.filters;
using Api_1.Entity.Pagnations;
using Api_1.Entity.Rate_Limiter;
using Api_1.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api_1.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableRateLimiting(RateLimiters.UserLimiter)]
public class StudentsController : ControllerBase
{
    private EducationalPlatformContext db;
    private StudentRepository student;
   
    public StudentsController(EducationalPlatformContext db, StudentRepository student)
    {
        this.db = db;
        this.student = student;   
    }

    [HttpGet]
    //[DisableCors]
    //[EnableCors("AllowAll2")]
    //[AllowAnonymous]
    [Authorize(Roles = DefaultRoles.Admin , AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[HasPermission(Permissions.GetStudents , AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    // [HasPermission(Permissions.updateStudent , AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    // [Authorize(Roles = DefaultRoles.Instructor , AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task <IActionResult> Get([FromQuery] RequestFilters requestFilters , CancellationToken cancellationToken)
    {
        var result = await student.all(requestFilters , cancellationToken);
        return Ok(result);
    }
    [HttpGet("/GetAllStudent")]
    public async Task<IActionResult> GetAllStudent(CancellationToken cancellationToken)
    {
        var result = await student.AllStudent( cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    //[Authorize]
    [Route("/getid")]
    [Authorize(Roles = DefaultRoles.Member, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetById()
    {
        // Extract the user ID from the token claims
        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }

        // Use the userId to query the database
        var result = await student.GetById(userId);

        if (result is not null)
        {
            return Ok(result);
        }

        return NotFound("Student not found.");
    }
    [HttpGet]
    [Route("{name:alpha}")]
    public IActionResult GetByName([FromRoute] string name)
    {
        var result = db.Students.FirstOrDefault(s => s.FullName == name);
        return Ok(result.mapp_to_student_Response());

    }

    [HttpPut]
    [Route("{Id}")]
    public async Task<IActionResult> updateStudent(string Id, string name , CancellationToken cancellationToken )
    {
        var isvalled = await student.update(Id, name , cancellationToken);
        //if (!isvalled)
        //    return NotFound();
        //return CreatedAtAction(nameof(GetById) ,new { Id = Id});
        return RedirectToAction(nameof(GetById), new { Id = Id });

    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(string id , CancellationToken cancellationToken = default)
    {
        var isvalled = await student.Delete(id , cancellationToken);
        if (!isvalled)
            return NotFound();
        return NoContent();

    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Add(Student newstudent, CancellationToken cancellationToken)
    {
        var result = await student.add(newstudent, cancellationToken);
        if (result.Password != null)
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        else
            return BadRequest("this quey is unique");
    }



}
