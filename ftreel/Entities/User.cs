﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ftreel.Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Mail { get; set; } = "";

    public string Password { get; set; } = "";

    public IList<string>? Roles { get; set; } = new List<string>();
}