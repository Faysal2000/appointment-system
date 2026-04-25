// Data/ApplicationDbContext.cs
using AppointmentSystem.Models;
using AppointmentSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<DoctorService> DoctorServices { get; set; }
        public DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── User ──────────────────────────────────────
            modelBuilder.Entity<User>(e =>
            {
                e.Property(u => u.FullName).IsRequired().HasMaxLength(100);
                e.Property(u => u.Email).IsRequired().HasMaxLength(100);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Password).IsRequired();
                e.Property(u => u.Phone).HasMaxLength(20);
            });

            // ── Role ──────────────────────────────────────
            modelBuilder.Entity<Role>(e =>
            {
                e.Property(r => r.Name).IsRequired().HasMaxLength(50);
            });

            // ── Doctor ────────────────────────────────────
            modelBuilder.Entity<Doctor>(e =>
            {
                e.HasOne(d => d.User)
                 .WithOne(u => u.Doctor)
                 .HasForeignKey<Doctor>(d => d.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Patient ───────────────────────────────────
            modelBuilder.Entity<Patient>(e =>
            {
                e.Property(p => p.FullName).IsRequired().HasMaxLength(100);
                e.Property(p => p.Phone).HasMaxLength(20);
                e.Property(p => p.Gender).HasConversion<string>();
            });

            // ── Service ───────────────────────────────────
            modelBuilder.Entity<Service>(e =>
            {
                e.Property(s => s.Name).IsRequired().HasMaxLength(100);
            });

            // ── DoctorService ─────────────────────────────
            modelBuilder.Entity<DoctorService>(e =>
            {
                e.Property(ds => ds.Price).HasColumnType("decimal(10,2)");
                e.HasOne(ds => ds.Doctor)
                 .WithMany(d => d.DoctorServices)
                 .HasForeignKey(ds => ds.DoctorId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(ds => ds.Service)
                 .WithMany(s => s.DoctorServices)
                 .HasForeignKey(ds => ds.ServiceId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── DoctorSpecialty ───────────────────────────
            modelBuilder.Entity<DoctorSpecialty>(e =>
            {
                e.HasOne(ds => ds.Doctor)
                 .WithMany(d => d.DoctorSpecialties)
                 .HasForeignKey(ds => ds.DoctorId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(ds => ds.Specialty)
                 .WithMany(s => s.DoctorSpecialties)
                 .HasForeignKey(ds => ds.SpecialtyId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── DoctorSchedule ────────────────────────────
            modelBuilder.Entity<DoctorSchedule>(e =>
            {
                e.HasOne(ds => ds.Doctor)
                 .WithMany(d => d.DoctorSchedules)
                 .HasForeignKey(ds => ds.DoctorId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Appointment ───────────────────────────────
            modelBuilder.Entity<Appointment>(e =>
            {
                e.Property(a => a.Status).HasConversion<string>();
                e.HasOne(a => a.Doctor)
                 .WithMany(d => d.Appointments)
                 .HasForeignKey(a => a.DoctorId)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(a => a.Patient)
                 .WithMany(p => p.Appointments)
                 .HasForeignKey(a => a.PatientId)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(a => a.DoctorService)
                 .WithMany(ds => ds.Appointments)
                 .HasForeignKey(a => a.DoctorServiceId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Invoice ───────────────────────────────────
            modelBuilder.Entity<Invoice>(e =>
            {
                e.Property(i => i.TotalAmount).HasColumnType("decimal(10,2)");
                e.Property(i => i.Status).HasMaxLength(20);
                e.HasOne(i => i.Appointment)
                 .WithOne(a => a.Invoice)
                 .HasForeignKey<Invoice>(i => i.AppointmentId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Payment ───────────────────────────────────
            modelBuilder.Entity<Payment>(e =>
            {
                e.Property(p => p.Amount).HasColumnType("decimal(10,2)");
                e.Property(p => p.PaymentMethod).HasConversion<string>();
                e.HasOne(p => p.Invoice)
                 .WithMany(i => i.Payments)
                 .HasForeignKey(p => p.InvoiceId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}