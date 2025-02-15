using Api_1.Contract;
using Api_1.Entity;
using Api_1.Entity.Consts;
using Api_1.Entity.Pagnations;
using Api_1.Model;
using Api_1.Outherize;
using Api_1.Settings;
using Hangfire;
using Mapster;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Linq.Dynamic.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Api_1.Repository;

public class StudentRepository(EducationalPlatformContext _db, token token,
    UserManager<Student> user
    , SignInManager<Student> signInManager
    , ILogger<StudentRepository> logger
    , IEmailSender emailSender
    , CacheService cacheService
    , IHttpContextAccessor httpContextAccessor
    )
{
    private readonly EducationalPlatformContext db = _db;
    public token Token = token;
    private readonly UserManager<Student> _userManager = user;
    private readonly SignInManager<Student> signInManager = signInManager;
    private readonly ILogger<StudentRepository> logger = logger;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly CacheService cacheService = cacheService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly int RefreshTokenDays = 14;

    public async Task<PaginatedList<Student>> all(RequestFilters requestFilters , CancellationToken cancellationToken = default)
    {
        //var query = db.Students.Where(i => i.IsDelete == false);
        
        // use cahing to get data from database by disctributed cache
        var cacheKey = 
            $"students_{requestFilters.PageNumber}_{requestFilters.PageSize}_{requestFilters.SearchValue}_{requestFilters.SortColumn}_{requestFilters.SortDirection}";
        

        var allStudents = await cacheService.GetAsync<IEnumerable<Student>>(cacheKey, cancellationToken);
        IEnumerable<Student> query = null!;

        if (allStudents is null)
        {
            query = db.Students.Where(i => i.IsDelete == false);
            await cacheService.SetAsync(cacheKey, query, cancellationToken);
            // use this to filter data by search value
            if (!string.IsNullOrWhiteSpace(requestFilters.SearchValue))
            {
                query = query.Where(i => i.FullName!.Contains(requestFilters.SearchValue));
            }

            // use this to sort data by column or date
            query = query.AsQueryable().OrderBy($"{requestFilters.SortColumn} {requestFilters.SortDirection}");
        }
        else
        {
             query =  allStudents; 
        }

        var result = await PaginatedList<Student>.CreateAsync(query, requestFilters.PageNumber, requestFilters.PageSize);
        return result!;
    }

    public async Task<IEnumerable<Student>> AllStudent(CancellationToken cancellationToken = default)
    { 
        //var query = db.Students.Where(i => i.IsDelete == false);
        // use cahing to get data from database by disctributed cache
        var cacheKey = $"students_without_use_Pagination";
        var allStudents = await cacheService.GetAsync<IEnumerable<Student>>(cacheKey, cancellationToken);
        IEnumerable<Student> query = null!;
        if (allStudents is null)
        {
            query = db.Students.Where(i => i.IsDelete == false);
            await cacheService.SetAsync(cacheKey, query, cancellationToken);
}
        else
        {
            query = allStudents;
        }
        return query!;
    }

    public async Task<Student> GetById(string id )
        => await db.Students.SingleOrDefaultAsync(i => i.Id == id );

    public async Task<Student> add(Student student, CancellationToken cancellationToken = default)
    {
        var isvaliad = await db.Students.AnyAsync(i => i.Password == student.Password && i.Email == student.Email);
        if (isvaliad)
            return new Student { };
        await db.Students.AddAsync(student, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        // use to cahing data by disctributed cache
        await cacheService.RemoveAsync($"students_without_use_Pagination", cancellationToken);

        return student;
    }

    public async Task<bool> update(string id, string name, CancellationToken cancellationToken = default)
    {
        //var isfind = await GetById(id);
        var isfind = await db.Students.Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(x => x.FullName, name)
            );

        await db.SaveChangesAsync(cancellationToken);
        // use to cahing data by disctributed cache
        await cacheService.RemoveAsync($"students_without_use_Pagination", cancellationToken);
        return true;
    }
    public async Task<bool> Delete(string id, CancellationToken cancellationToken = default)
    {
        var isfind = await GetById(id);
        if (isfind is null)
            return false;
        //db.Students.Remove(isfind);
        isfind.IsDelete = true;
        await db.SaveChangesAsync(cancellationToken);

        // use to cahing data by disctributed cache
        await cacheService.RemoveAsync($"students_without_use_Pagination", cancellationToken);
        return true;
    }



    public async Task<StudentResponse?> Login(string Email, string password, CancellationToken cancellationToken = default)
    {

        
        var user = await db.Students.SingleOrDefaultAsync(i => i.Email == Email , cancellationToken);
       
        if (user is null)
            return null;
       var isValidPassword = await signInManager.PasswordSignInAsync(user, password, false, true);

        if (user.IsDelete == true)
            return null;

        if (user.EmailConfirmed == false)
            return new StudentResponse { id = "Email Not Confirmed" } ;

        if (user.LockoutEnd != null)
        {
            if (user.LockoutEnd > DateTime.UtcNow)
                return new StudentResponse { id = "Looked user For 5 Minutes" };
        }
        if (!isValidPassword.Succeeded)
            return null;

        var result = user.Adapt<StudentResponse>();
        result.id = user.Id;

        var (userRoles , userPermissions) = await GetUserRolesAndPermissions(user, cancellationToken);
       // var userRoles = await _userManager.GetRolesAsync(user);

        var (token, expiresIn) = Token.GenerateToken(result, userRoles , userPermissions);
        result.Token = token;
        result.ExpiresIn = expiresIn;

        result.RefreshToken = GenerateRefreshToken();
        result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = result.RefreshToken,
            ExpiresOn = result.RefreshTokenExpiretion,
        });
        await db.SaveChangesAsync(cancellationToken);
        // await _userManager.UpdateAsync(user);
        return result;

    }
    public async Task<StudentResponse?> GetRefreshTokenAsync(string tokenn, string refrehToken, CancellationToken cancellationToken)
    {


        var userId = Token.ValisationToken(tokenn);

        if (userId is null)
            return null;

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null || user.IsDelete == true)
            return null;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refrehToken && x.IsActive);

        if (userRefreshToken is null)
            return null;



        // ***to stop Refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var result = user.Adapt<StudentResponse>();
        result.id = user.Id;
        var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellationToken);

        var (newtoken, expiresIn) = Token.GenerateToken(result ,userRoles , userPermissions );
        result.Token = newtoken;
        result.ExpiresIn = expiresIn;

        result.RefreshToken = GenerateRefreshToken();
        result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = result.RefreshToken,
            ExpiresOn = result.RefreshTokenExpiretion
        });
        await db.SaveChangesAsync(cancellationToken);
        await _userManager.UpdateAsync(user);


        return result;
    }

    public async Task<Student> RegisterAsync(Student request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            logger.LogError("Email is required.");
            return null;
        }
        // var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

        var emailIsExists = await db.Students.AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (emailIsExists)
            return null;


        request.UserName = request.Email;
        // var save = await db.Students.AddAsync(request, cancellationToken);

        var save = await _userManager.CreateAsync(request, request.Password);
        await db.SaveChangesAsync(cancellationToken);
        if (save.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(request);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            logger.LogInformation("Confirmation code: {code}", code);
            await SendConfirmationEmail(request, code);


            // use to cahing data by disctributed cache
            await cacheService.RemoveAsync($"students_without_use_Pagination", cancellationToken);

            return request;
        }

        var error = save.Errors.ToList();
        logger.LogError("User creation failed: {error}", error[0].Description);

        return new Student { FullName = "username must unique" };
    }




    public async Task<string> ConfirmEmailAsync(ConfirmEmailRequest request)
    {

        var user = await db.Students.SingleOrDefaultAsync(i => i.Id == request.id);

        // var user = await _userManager.FindByIdAsync(request.id);
        if (user is null)
            return null!;

        if (user.EmailConfirmed)
            return  "your Email was Confirmed";

        var DEcode = request.code;

        try
        {
            DEcode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(DEcode));
        }
        catch (FormatException)
        {
            return null!;
        }

        var result = await _userManager.ConfirmEmailAsync(user, DEcode);


        if (result.Succeeded)
        {
            //user.EmailConfirmed = true;
            await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
            return "done";
        }
        //var userClaims = await _userManager.GetClaimsAsync(user).;
        var error = result.Errors.First();

        return null!;
    }

    public async Task<Student> ResendEmailConfirmation(ReConfirm_email request)
    {


        var user = await db.Students.SingleOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
            return null!;
        if (user.EmailConfirmed)
            return new Student { FullName = "your Email was Confirmed" };

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        logger.LogInformation("Confirmation code: {code}", code);

        await SendConfirmationEmail(user, code);

        return user;

    }

    public async Task<bool> ForgetPasswordAsny(string email)
    {
        var user = await db.Students.SingleOrDefaultAsync(x => x.Email == email);
        if (user == null || !user.EmailConfirmed)
            return false;

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        logger.LogInformation("Reset code: {code}", code);

        await SendResetPasswordEmail(user, code);

        return true;


    }

    public async Task<bool> ResetPasswordAsync(ResetPassword request )
    {
        var user = await db.Students.SingleOrDefaultAsync( x => x.Email == request.Email);
        if (user == null || !user.EmailConfirmed)
            return false;
        IdentityResult result;

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
            user.Password = request.NewPassword;
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if (result.Succeeded)
        {
            await db.SaveChangesAsync();
            return true;
        }

        var error = result.Errors.First();
        return false; 
    }

    private async Task SendConfirmationEmail(Student user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            templateModel: new Dictionary<string, string>
            {
                { "{{name}}", user.FullName! },
                    { "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}" }
            }
        );

        // to use backgroud jobs make code fast 
        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Education Platform : Email Confirmation", emailBody));
        await Task.CompletedTask;

        // if you didnot use background jobs 
       // await _emailSender.SendEmailAsync(user.Email!, "✅ Education Platform : Email Confirmation", emailBody);
    }

    private async Task SendResetPasswordEmail(Student user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
            templateModel: new Dictionary<string, string>
            {
                { "{{name}}", user.FullName! },
                { "{{action_url}}", $"{origin}/auth/forgetPassword?email={user.Email}&code={code}" }
            }
        );

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅  Education Platform: Change Password", emailBody));

        await Task.CompletedTask;
    }

    private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissions(Student user, CancellationToken cancellationToken)
    {
        var userRoles = (await _userManager.GetRolesAsync(user)).ToList();

        var allPermissions = await db.Roles
            .Join(db.RoleClaims,
                role => role.Id,
                claim => claim.RoleId,
                (role, claim) => new { role, claim }
            ).ToListAsync(cancellationToken);
        
        var userPermissions = allPermissions.Where(x => userRoles.Contains(x.role.Name!))
            .Select(x => x.claim.ClaimValue!)
            .Distinct()
            .ToList();
        //var userPermissions = await (from r in db.Roles
        //                             join p in db.RoleClaims
        //                             on r.Id equals p.RoleId
        //                             where userRoles.Contains(r.Name!)
        //                             select p.ClaimValue!)
        //                             .Distinct()
        //                             .ToListAsync(cancellationToken);



        return (userRoles, userPermissions);
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(count: 64));
    }

}
