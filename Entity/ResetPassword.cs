using System.ComponentModel.DataAnnotations;

namespace Api_1.Entity;

public class ResetPassword
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Code { get; set; }
    [Required]
    [MinLength(3)]
    public string NewPassword { get; set; }

}
