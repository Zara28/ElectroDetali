﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ElectroDetali.DBModels;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public string Code { get; set; }

    public bool? Isapp { get; set; }

    public virtual ICollection<Buy> Buys { get; set; } = new List<Buy>();
}