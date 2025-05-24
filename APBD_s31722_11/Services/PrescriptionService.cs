using System.Net;
using APBD_s31722_11.DataLayer.Models;
using APBD_s31722_11.Interfaces;
using Microsoft.EntityFrameworkCore;
using APBD_s31722_11.DataLayer;

namespace APBD_s31722_11.Services;

public class PrescriptionService : IPrescriptionService

{
    private readonly DatabaseContext _context;

    public PrescriptionService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> AddPrescriptionAsync(PrescriptionRequestDto dto)
    {
        // validating
        await ValidateMedicamentsAsync(dto);
        ValidateDueDate(dto);
        var doctor = await TryGetDoctor(dto);
        var patient = await GetOrAddPatient(dto);

        // mapping
        var prescription = MapPrescription(dto, patient, doctor);
        
        //saving
        await SaveToDb(prescription);
        return prescription.IdPrescription;
    }

    private async Task SaveToDb(Prescription prescription)
    {
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }

    private static PrescriptionMedicament MapPrescriptionMedicament(PrescriptionRequestDto.MedicamentDto mDto)
    {
        return new PrescriptionMedicament
        {
            IdMedicament = mDto.IdMedicament,
            Dose = mDto.Dose,
            Details = mDto.Description
        };
    }

    private static Prescription MapPrescription(PrescriptionRequestDto dto, Patient patient, Doctor doctor)
    {
        return new Prescription
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            Date = dto.Date,
            DueDate = dto.DueDate,
            PrescriptionMedicaments = dto.Medicaments.Select(MapPrescriptionMedicament).ToList()
        };
    }

    private async Task<Doctor> TryGetDoctor(PrescriptionRequestDto dto)
    {
        return await _context.Doctors.FindAsync(dto.Doctor.IdDoctor)
               ?? throw new HttpRequestException($"Doctor with id {dto.Doctor.IdDoctor} not found", 
                   null,
                   HttpStatusCode.NotFound);
    }

    private async Task<Patient> GetOrAddPatient(PrescriptionRequestDto dto)
    {
        Patient? patient = null;
        if (dto.Patient.IdPatient != null && dto.Patient.IdPatient > 0)
        {
            patient = await _context.Patients.FindAsync(dto.Patient.IdPatient);
        }

        if (patient == null)
        {
            if (dto.Patient.IdPatient != null)
            {
                if (!dto.Patient.DateOfBirth.HasValue)
                {
                    throw new HttpRequestException("No date of birth", null, HttpStatusCode.BadRequest);
                }
            }
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                DateOfBirth = dto.Patient.DateOfBirth,
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }
        return patient;
    }

    private static void ValidateDueDate(PrescriptionRequestDto dto)
    {
        if (dto.DueDate < dto.Date)
        {
            throw new HttpRequestException("Due date cannot be earlier than date", null, HttpStatusCode.BadRequest);
        }
    }

    private async Task ValidateMedicamentsAsync(PrescriptionRequestDto dto)
    {
        if (dto.Medicaments == null || dto.Medicaments.Count == 0)
        {
            throw new HttpRequestException("No medicaments", null, HttpStatusCode.BadRequest);
        }

        if (dto.Medicaments.Count > 10)
        {
            throw new HttpRequestException("Too many Medicaments, max - 10", null, HttpStatusCode.BadRequest);
        }
        
        foreach (var mDto in dto.Medicaments)
        {
            var med = await _context.Medicaments.FindAsync(mDto.IdMedicament);
            if (med == null)
            {
                throw new HttpRequestException($"Medicament with id {mDto.IdMedicament} does not exist");
            }
        }

    }
}