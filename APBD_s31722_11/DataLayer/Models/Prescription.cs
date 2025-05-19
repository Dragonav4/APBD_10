using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace APBD_s31722_11.DataLayer.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    [ForeignKey(nameof(PatientId))]
    public int PatientId { get; set; }
    [ForeignKey(nameof(DoctorId))]
    public int DoctorId { get; set; }
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    public Prescription()
    {
    }
}