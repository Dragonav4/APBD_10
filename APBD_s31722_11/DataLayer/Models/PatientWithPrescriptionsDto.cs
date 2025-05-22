namespace APBD_s31722_11.DataLayer.Models
{
    public class PatientWithPrescriptionsDto
    {
        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public List<PrescriptionDto> Prescriptions { get; set; }
            = new List<PrescriptionDto>();
    }

    public class PrescriptionDto
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        public DoctorDto Doctor { get; set; }

        public List<MedicamentDto> Medicaments { get; set; }
            = new List<MedicamentDto>();
    }

    public class DoctorDto
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class MedicamentDto
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public int Dose { get; set; }
        public string Description { get; set; }
    }
}