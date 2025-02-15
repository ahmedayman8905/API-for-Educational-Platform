using System;
using System.Collections.Generic;

namespace Api_1.Model;

public partial class Material
{
    public int Id { get; set; }

    public string? FilePath { get; set; }

    public int? CoursId { get; set; }

    public string? LecuerNumber { get; set; }

    public virtual Course? Cours { get; set; }
}
