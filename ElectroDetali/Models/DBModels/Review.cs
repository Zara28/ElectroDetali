﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ElectroDetali.Models;

public partial class Review
{
    public int Id { get; set; }

    public int? Goodid { get; set; }

    public int? Userid { get; set; }

    public string Value { get; set; }

    public virtual Good Good { get; set; }

    public virtual User User { get; set; }
}