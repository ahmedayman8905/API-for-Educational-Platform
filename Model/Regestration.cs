using System;
using System.Collections.Generic;

namespace Api_1.Model;

public partial class Regestration
{
    public string StudentId { get; set; }

    public int CourseId { get; set; }

    public DateTime? StarTdate { get; set; } = DateTime.UtcNow;

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
