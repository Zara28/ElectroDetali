﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using ElectroDetali.Models.HelperModels;
using Microsoft.EntityFrameworkCore;

namespace ElectroDetali.Models;

public partial class ElectroDetaliContext : DbContext
{
    public ElectroDetaliContext()
    {
    }

    public ElectroDetaliContext(DbContextOptions<ElectroDetaliContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Buy> Buys { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Good> Goods { get; set; }

    public virtual DbSet<PickUpPoint> PickUpPoints { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql(VariablesStorage.GetVariable("DB_CONNECTION"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Buy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("buy_pkey");

            entity.ToTable("buy");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Datecreate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datecreate");
            entity.Property(e => e.Datedelivery)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datedelivery");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Goodid).HasColumnName("goodid");
            entity.Property(e => e.Isbasket).HasColumnName("isbasket");
            entity.Property(e => e.Pointid).HasColumnName("pointid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Good).WithMany(p => p.Buys)
                .HasForeignKey(d => d.Goodid)
                .HasConstraintName("buy_goodid_fkey");

            entity.HasOne(d => d.Point).WithMany(p => p.Buys)
                .HasForeignKey(d => d.Pointid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("buy_pointid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Buys)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("buy_userid_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Good>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("good_pkey");

            entity.ToTable("good");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.Category).WithMany(p => p.Goods)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("good_categoryid_fkey");
        });

        modelBuilder.Entity<PickUpPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pick_up_point_pkey");

            entity.ToTable("pick_up_point");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasMaxLength(200)
                .HasColumnName("adress");
            entity.Property(e => e.Time).HasColumnName("time");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("review_pkey");

            entity.ToTable("review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Goodid).HasColumnName("goodid");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Good).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Goodid)
                .HasConstraintName("review_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("review_fk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Isadmin).HasColumnName("isadmin");
            entity.Property(e => e.Isapp).HasColumnName("isapp");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}