using System;
using System.Collections.Generic;

namespace Integration.Models;

public partial class BLUser
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }
    public string? SecurityToken { get; set; }

    public bool IsConfirmed { get; set; }

    public virtual Country CountryOfResidence { get; set; } = null!;
}
