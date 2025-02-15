using System;
using System.Collections.Generic;

namespace Api_1;

public partial class Instructor
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime? JoinDate { get; set; } = DateTime.UtcNow;  

    public string? Gender { get; set; }

    public string? Image { get; set; }

    public bool? IsDelete { get; set; } = false;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
