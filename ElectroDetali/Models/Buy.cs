﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ElectroDetali.Models;

public partial class Buy
{
    public int Id { get; set; }

    public int? Goodid { get; set; }

    public int? Userid { get; set; }

    public string Email { get; set; }

    public DateTime? Datecreate { get; set; }

    public DateTime? Datedelivery { get; set; }

    public bool? Isbasket { get; set; }

    public int Pointid { get; set; }

    public virtual Good Good { get; set; }

    public virtual PickUpPoint Point { get; set; }

    public virtual User User { get; set; }
}