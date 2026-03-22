using Microsoft.EntityFrameworkCore;
using ScaffoldingSystem.Domain.Entities;

namespace ScaffoldingSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Scaffolding> Scaffoldings => Set<Scaffolding>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<RentalItem> RentalItems => Set<RentalItem>();
    public DbSet<Dispatch> Dispatches => Set<Dispatch>();
    public DbSet<Reception> Receptions => Set<Reception>();
    public DbSet<Inspection> Inspections => Set<Inspection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── USER ──────────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).HasMaxLength(150);
            e.Property(u => u.FullName).HasMaxLength(100);
            e.Property(u => u.Phone).HasMaxLength(20);
        });

        // ── SCAFFOLDING ───────────────────────────────────────────────────────
        modelBuilder.Entity<Scaffolding>(e =>
        {
            e.HasIndex(s => s.Code).IsUnique();
            e.Property(s => s.Code).HasMaxLength(20);
            e.Property(s => s.HeightMeters).HasPrecision(5, 2);
            e.Property(s => s.WeightKg).HasPrecision(7, 2);
        });

        // ── RENTAL ────────────────────────────────────────────────────────────
        modelBuilder.Entity<Rental>(e =>
        {
            e.HasIndex(r => r.RentalNumber).IsUnique();
            e.Property(r => r.TotalAmount).HasPrecision(12, 2);
            e.HasOne(r => r.Client)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.ClientUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── RENTAL ITEM ───────────────────────────────────────────────────────
        modelBuilder.Entity<RentalItem>(e =>
        {
            e.Property(i => i.DailyRate).HasPrecision(10, 2);
            e.HasOne(i => i.Rental)
                .WithMany(r => r.Items)
                .HasForeignKey(i => i.RentalId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(i => i.Scaffolding)
                .WithMany(s => s.RentalItems)
                .HasForeignKey(i => i.ScaffoldingId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── DISPATCH ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Dispatch>(e =>
        {
            e.HasOne(d => d.Rental)
                .WithMany(r => r.Dispatches)
                .HasForeignKey(d => d.RentalId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(d => d.DispatchedBy)
                .WithMany(u => u.Dispatches)
                .HasForeignKey(d => d.DispatchedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── RECEPTION ─────────────────────────────────────────────────────────
        modelBuilder.Entity<Reception>(e =>
        {
            e.HasOne(r => r.Dispatch)
                .WithOne(d => d.Reception)
                .HasForeignKey<Reception>(r => r.DispatchId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(r => r.ReceivedBy)
                .WithMany(u => u.Receptions)
                .HasForeignKey(r => r.ReceivedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── INSPECTION ────────────────────────────────────────────────────────
        modelBuilder.Entity<Inspection>(e =>
        {
            e.HasOne(i => i.Scaffolding)
                .WithMany(s => s.Inspections)
                .HasForeignKey(i => i.ScaffoldingId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(i => i.Inspector)
                .WithMany(u => u.Inspections)
                .HasForeignKey(i => i.InspectorUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
