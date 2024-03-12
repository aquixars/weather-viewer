using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestTasks.DS.WeatherViewer.Models.DBEntities;

public partial class WeatherViewerDbContext : DbContext
{
    public WeatherViewerDbContext()
    {
    }

    public WeatherViewerDbContext(DbContextOptions<WeatherViewerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<WeatherArchiveRecord> WeatherArchiveRecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(SecretsReader.ReadSection<string>("dbConnectionString"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherArchiveRecord>(entity =>
        {
            entity.ToTable("WeatherArchiveRecords", "weatherViewer");

            entity.HasIndex(e => e.Created, "IX_WeatherArchiveRecords_created");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CloudBase).HasColumnName("cloudBase");
            entity.Property(e => e.Cloudiness).HasColumnName("cloudiness");
            entity.Property(e => e.Created)
                .HasColumnType("datetime")
                .HasColumnName("created");
            entity.Property(e => e.DewPoint)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("dewPoint");
            entity.Property(e => e.HorizontalVisibility)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("horizontalVisibility");
            entity.Property(e => e.Humidity)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("humidity");
            entity.Property(e => e.Pressure).HasColumnName("pressure");
            entity.Property(e => e.Temperature)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("temperature");
            entity.Property(e => e.WeatherСonditions)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("weatherСonditions");
            entity.Property(e => e.WindDirection)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("windDirection");
            entity.Property(e => e.WindSpeed).HasColumnName("windSpeed");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
