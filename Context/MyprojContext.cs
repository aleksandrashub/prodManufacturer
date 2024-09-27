using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProdManufacturer.Models;

namespace ProdManufacturer.Context;

public partial class MyprojContext : DbContext
{
    public MyprojContext()
    {
    }

    public MyprojContext(DbContextOptions<MyprojContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DopImg> DopImgs { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleTovar> SaleTovars { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Tovar> Tovars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=myproj;Username=postgres;Port=5433;Password=18b22M02a");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DopImg>(entity =>
        {
            entity.HasKey(e => e.IdDopImg).HasName("dop_imgs_pk");

            entity.ToTable("dop_imgs");

            entity.Property(e => e.IdDopImg).HasColumnName("id_dop_img");
            entity.Property(e => e.IdTovar).HasColumnName("id_tovar");
            entity.Property(e => e.NameImg)
                .HasColumnType("character varying")
                .HasColumnName("name_img");

            entity.HasOne(d => d.IdTovarNavigation).WithMany(p => p.DopImgs)
                .HasForeignKey(d => d.IdTovar)
                .HasConstraintName("dop_imgs_tovar_fk");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.IdManufacturer).HasName("manufacturer_pk");

            entity.ToTable("manufacturer");

            entity.Property(e => e.IdManufacturer).HasColumnName("id_manufacturer");
            entity.Property(e => e.NameManufacturer)
                .HasColumnType("character varying")
                .HasColumnName("name_manufacturer");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.IdSale).HasName("sales_pk");

            entity.ToTable("sales");

            entity.Property(e => e.IdSale).HasColumnName("id_sale");
            entity.Property(e => e.Date).HasColumnName("date");
        });

        modelBuilder.Entity<SaleTovar>(entity =>
        {
            entity.HasKey(e => new { e.IdSale, e.IdTovar }).HasName("sale_tovar_pk");

            entity.ToTable("sale_tovar");

            entity.Property(e => e.IdSale).HasColumnName("id_sale");
            entity.Property(e => e.IdTovar).HasColumnName("id_tovar");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.IdSaleNavigation).WithMany(p => p.SaleTovars)
                .HasForeignKey(d => d.IdSale)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_tovar_sales_fk");

            entity.HasOne(d => d.IdTovarNavigation).WithMany(p => p.SaleTovars)
                .HasForeignKey(d => d.IdTovar)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_tovar_tovar_fk");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("newtable_pk");

            entity.ToTable("status");

            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.NameStatus)
                .HasColumnType("character varying")
                .HasColumnName("name_status");
        });

        modelBuilder.Entity<Tovar>(entity =>
        {
            entity.HasKey(e => e.IdTovar).HasName("tovar_pk");

            entity.ToTable("tovar");

            entity.Property(e => e.IdTovar).HasColumnName("id_tovar");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.IdManufacturer).HasColumnName("id_manufacturer");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.NameTovar)
                .HasColumnType("character varying")
                .HasColumnName("name_tovar");

            entity.HasOne(d => d.IdManufacturerNavigation).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.IdManufacturer)
                .HasConstraintName("tovar_manufacturer_fk");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("tovar_status_fk");

            entity.HasMany(d => d.IdDopTovs).WithMany(p => p.IdMainTovs)
                .UsingEntity<Dictionary<string, object>>(
                    "TovarDop",
                    r => r.HasOne<Tovar>().WithMany()
                        .HasForeignKey("IdDopTov")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("tovar_dop_tovar_fk"),
                    l => l.HasOne<Tovar>().WithMany()
                        .HasForeignKey("IdMainTov")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("tovar_dop_tovar_fk_1"),
                    j =>
                    {
                        j.HasKey("IdMainTov", "IdDopTov").HasName("tovar_dop_pk");
                        j.ToTable("tovar_dop");
                        j.IndexerProperty<int>("IdMainTov").HasColumnName("id_main_tov");
                        j.IndexerProperty<int>("IdDopTov").HasColumnName("id_dop_tov");
                    });

            entity.HasMany(d => d.IdMainTovs).WithMany(p => p.IdDopTovs)
                .UsingEntity<Dictionary<string, object>>(
                    "TovarDop",
                    r => r.HasOne<Tovar>().WithMany()
                        .HasForeignKey("IdMainTov")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("tovar_dop_tovar_fk_1"),
                    l => l.HasOne<Tovar>().WithMany()
                        .HasForeignKey("IdDopTov")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("tovar_dop_tovar_fk"),
                    j =>
                    {
                        j.HasKey("IdMainTov", "IdDopTov").HasName("tovar_dop_pk");
                        j.ToTable("tovar_dop");
                        j.IndexerProperty<int>("IdMainTov").HasColumnName("id_main_tov");
                        j.IndexerProperty<int>("IdDopTov").HasColumnName("id_dop_tov");
                    });
        });
        modelBuilder.HasSequence("manufacturer_id_manufacturer_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("newtable_id_status_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("sales_id_sale_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("tovar_dop_id_main_dop_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("tovar_id_tovar_seq").HasMax(2147483647L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
