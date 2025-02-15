using System.ComponentModel.DataAnnotations;

namespace Api_1.Entity;

public class ReConfirm_email
{
    [Required]
    public string Email { get; set; }
}
