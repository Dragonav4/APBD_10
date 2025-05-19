using APBD_s31722_11.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_s31722_11.DataLayer;

//1 make this DbContext
//use commands for migrations:
// dotnet ef database update/remove/drop
//dotnet ef migrations add InitialCreate
public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(prescriptionMedicament =>
                new
                {
                    prescriptionMedicament.IdPrescription,
                    prescriptionMedicament.IdMedicament
                });
        
        
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdMedicament);
        
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription);

        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { Id = 1, FirstName = "Kowalski",    LastName = "NieGlowny", Email = "Kowalski@gmail.com" },
            new Doctor { Id = 2, FirstName = "Nowak",       LastName = "Lekarz",    Email = "nowak@example.com" },
            new Doctor { Id = 3, FirstName = "Lewandowski", LastName = "Drugi",     Email = "lewandowski@example.com" }
        );

        modelBuilder.Entity<Patient>().HasData(
            new Patient { Id = 1, FirstName = "Szkiper", LastName = "Glowny", DateOfBirth = new DateTime(2000, 1, 1) },
            new Patient { Id = 2, FirstName = "Anna",    LastName = "Kowalska", DateOfBirth = new DateTime(1995, 2, 15) },
            new Patient { Id = 3, FirstName = "Piotr",   LastName = "Nowak",    DateOfBirth = new DateTime(1988, 7, 30) }
        );

        modelBuilder.Entity<Prescription>().HasData(
            new Prescription { IdPrescription = 1, Date = new DateTime(2025, 5, 19), DueDate = new DateTime(2025, 6, 19), PatientId = 1, DoctorId = 1 },
            new Prescription { IdPrescription = 2, Date = new DateTime(2025, 5, 20), DueDate = new DateTime(2025, 6, 20), PatientId = 2, DoctorId = 2 },
            new Prescription { IdPrescription = 3, Date = new DateTime(2025, 5, 21), DueDate = new DateTime(2025, 6, 21), PatientId = 3, DoctorId = 3 }
        );

        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "Elixir",     Type = "HP potion",    Description = "Cool medicament from all bad stuff" },
            new Medicament { IdMedicament = 2, Name = "Antibiotic", Type = "Antibiotic",   Description = "Fights bacterial infections" },
            new Medicament { IdMedicament = 3, Name = "Painkiller", Type = "Analgesic",    Description = "Reduces pain levels" }
        );

        modelBuilder.Entity<PrescriptionMedicament>().HasData(
            new PrescriptionMedicament { IdPrescription = 1, IdMedicament = 1, Dose = 1, Details = "Elixir" },
            new PrescriptionMedicament { IdPrescription = 2, IdMedicament = 2, Dose = 2, Details = "Course of antibiotics" },
            new PrescriptionMedicament { IdPrescription = 3, IdMedicament = 3, Dose = 3, Details = "Pain relief course" }
        );
    }
    
    
}