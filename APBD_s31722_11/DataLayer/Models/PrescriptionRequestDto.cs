using System;
using System.Collections.Generic;

namespace APBD_s31722_11.Dto
{
    public class PrescriptionRequestDto
    {
        public PatientDto Patient { get; set; }
        public DoctorDto Doctor { get; set; }
        public List<MedicamentDto> Medicaments { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        public class PatientDto
        {
            public int? IdPatient { get; set; }    // если null или 0 — создаём нового
            public string FirstName { get; set; }
            public string LastName  { get; set; }
            public DateTime DateOfBirth { get; set; }
        }

        public class DoctorDto
        {
            public int IdDoctor { get; set; }
        }

        public class MedicamentDto
        {
            public int IdMedicament { get; set; }
            public int Dose         { get; set; }
            public string Description { get; set; }
        }
    }
}