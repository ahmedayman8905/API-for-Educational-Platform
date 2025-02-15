using Api_1.Entity;
using Api_1.Entity.Consts;
using Api_1.Entity.Rate_Limiter;
using Api_1.Outherize;
using Api_1.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace Api_1.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(RateLimiters.IpLimiter)]

public class outhenticationController(EducationalPlatformContext _db , StudentRepository student ) : ControllerBase
{
    private readonly EducationalPlatformContext db = _db;
    private readonly StudentRepository student = student;

    [HttpPost("/Login")]
    public async Task<IActionResult> Login(userLogin user , CancellationToken cancellationToken)
    {
        var isvalid = await student.Login(user.Email, user.Passsword , cancellationToken );
        if (isvalid == null)
            return NotFound("Invalid email/password");
        else if (isvalid.id == "Email Not Confirmed")
            return BadRequest("Email Not Confirmed");
        else if (isvalid.id == "Looked user For 5 Minutes")
            return BadRequest("Looked user For 5 Minutes");
        return Ok(isvalid);
    }
    [HttpPost("/RefreshToken")]
    public async Task <IActionResult> GetRefrehToken(RefreshtokenReqest request , CancellationToken cancellationToken)
    {
        // var isvalid = await student.Login(user.Email, user.Passsword);
        var isvalid = await student.GetRefreshTokenAsync(request.token, request.RefeshToken, cancellationToken);
        if (isvalid is null)
            return BadRequest("Invalid");
        return Ok(isvalid);

    }

    [HttpPost("/Register")]
    [DisableRateLimiting]
    public async Task<IActionResult> Registration(Student _student, CancellationToken cancellationToken)
    {
        var isvalid = await student.RegisterAsync(_student, cancellationToken);
        if (isvalid == null)
            return Conflict(new { message = "Email must be unique" });
        else if (isvalid.FullName == "username must unique")
            return Conflict(new { message = "username must unique" });
        return Ok(isvalid);
        
    }



    [HttpPost("/confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var result = await student.ConfirmEmailAsync(request);
        if (result != null)
        {
            if (result  == "your Email was Confirmed")
                return Content(result);
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost("/Resend-Confirm-email")]
    public async Task<IActionResult> RESENDConfirmEmail([FromBody] ReConfirm_email Email)
    {
        var result = await student.ResendEmailConfirmation(Email);
        if (result != null)
        {
            if (result.FullName == "your Email was Confirmed")
                return Content(result.FullName);
            return Ok();
        }
        return Content("not");
    }

    [HttpPost("/forgetPassword")]
    public async Task<IActionResult> forgetPassword([FromBody] string Email)
    {
        var result = await student.ForgetPasswordAsny(Email);
        return Ok();
    }

    [HttpPost("/Reset-forgetPassword")]
    public async Task<IActionResult> ResetforgetPassword([FromBody] ResetPassword request)
    {
        var result = await student.ResetPasswordAsync(request);
        if (result)
            return Ok();
        return NotFound("false code"); 


    }

    [HttpGet("/rate Limater test")]
    [EnableRateLimiting(RateLimiters.Concurrency)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
      //  var result = await student.GetStudentAsync(cancellationToken);
      Thread.Sleep(9000);
        return Ok();
    }


}
