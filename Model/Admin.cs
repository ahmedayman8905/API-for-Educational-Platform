using System;
using System.Collections.Generic;

namespace Api_1.Model;

public partial class Admin
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }
}
