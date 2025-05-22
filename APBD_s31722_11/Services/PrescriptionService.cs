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
        if (dto.Medicaments == null || dto.Medicaments.Count == 0)
        {
            throw new HttpRequestException("No medicaments", null, HttpStatusCode.BadRequest);
        }

        if (dto.Medicaments.Count > 10)
        {
            throw new HttpRequestException("Too many Medicaments, max - 10", null, HttpStatusCode.BadRequest);
        }

        if (dto.DueDate < dto.Date)
        {
            throw new HttpRequestException("Due date cannot be earlier than date", null, HttpStatusCode.BadRequest);
        }

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

        var doctor = await _context.Doctors.FindAsync(dto.Doctor.IdDoctor);
        if (doctor == null)
        {
            throw new HttpRequestException($"Doctor with id {dto.Doctor.IdDoctor} not found", null,
                HttpStatusCode.NotFound);
        }

        foreach (var mDto in dto.Medicaments)
        {
            var med = await _context.Medicaments.FindAsync(mDto.IdMedicament);
            if (med == null)
            {
                throw new HttpRequestException($"Medicament with id {mDto.IdMedicament} does not exist");
            }
        }

        var prescription = new Prescription
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            Date = dto.Date,
            DueDate = dto.DueDate,
            PrescriptionMedicaments = new List<PrescriptionMedicament>()
        };
        foreach (var mDto in dto.Medicaments)
        {
            prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament
                {
                    IdMedicament = mDto.IdMedicament,
                    Dose = mDto.Dose,
                    Details = mDto.Description
                }
            );
        }
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
        return prescription.IdPrescription;
    }

    public async Task<PatientWithPrescriptionsDto> GetPatientWithPrescriptionsAsync(int id)
    {
        var patient = await _context.Patients.SingleOrDefaultAsync(p => p.Id == id);
        if (patient == null) throw new HttpRequestException($"Patient with id {id} not found", null, HttpStatusCode.NotFound);
        var prescription = await _context.Prescriptions.Where(p => p.PatientId == id)
            .OrderBy(pr => pr.DueDate)
            .Include(pr => pr.Doctor)
            .Include(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .ToListAsync();
        
        var resultDto = new PatientWithPrescriptionsDto
        {
            IdPatient = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Prescriptions = prescription.Select(pr => new PrescriptionDto
            {
                IdPrescription = pr.IdPrescription,
                Date = pr.Date,
                DueDate = pr.DueDate,
                Doctor = new DoctorDto
                {
                    IdDoctor  = pr.DoctorId,
                    FirstName = pr.Doctor.FirstName,
                    LastName  = pr.Doctor.LastName
                },
                Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentDto
                {
                    IdMedicament = pm.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Description = pm.Details
                }).ToList()
            }).ToList()
        };
        return resultDto;
    }
}