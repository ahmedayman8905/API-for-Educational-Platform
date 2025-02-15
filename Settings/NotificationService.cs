using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Api_1.Settings;

public class NotificationService(
    EducationalPlatformContext context,
    UserManager<Student> userManager,
    IHttpContextAccessor httpContextAccessor,
    IEmailSender emailSender
    ): INotificationService
{
    private readonly EducationalPlatformContext db = context;
    private readonly UserManager<Student> userManager = userManager;
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
    private readonly IEmailSender emailSender = emailSender;

    

    public async Task SendNewCoursNotification()
    {

        //TODO: Select members only
        var users = await db.Students.ToListAsync();

        var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin;

        foreach (Student user in users)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "{{name}}", user.FullName! },
                { "{{pollTill}}", user.Phone! },
                { "{{endDate}}", user.JoinDate.ToString()! },
                { "{{url}}", $"{origin}/cours/start/{user.Id}" }
            };

            var body = EmailBodyBuilder.GenerateEmailBody("PollNotification", placeholders);

            await emailSender.SendEmailAsync(user.Email!, "📣 Education platform: New cours - {cours title}", body);
        }
  
    }
}
