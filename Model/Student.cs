using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api_1.Model;

public partial class Student : IdentityUser
{
   // public string Id { get; set; }

    public string? FullName { get; set; }

    [Required]
    public string Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? JoinDate { get; set; } = DateTime.UtcNow;

    public string Password { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDay { get; set; }

   // public new string? UserName { get; set; }

    public bool? IsDelete { get; set; } = false;

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];

    public virtual ICollection<Regestration> Regestrations { get; set; } = new List<Regestration>();
}
