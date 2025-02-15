using System;
using System.Collections.Generic;

namespace Api_1.Model;

public partial class Category
{
    public int Id { get; set; }

    public string? CatName { get; set; }

    public string? Image { get; set; }

    public bool IsDelete { get; set; } = false;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
