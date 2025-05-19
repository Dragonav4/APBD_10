using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_s31722_11.DataLayer.Models;

public class PrescriptionMedicament
{
    [ForeignKey(nameof(Medicament))]
    public int IdMedicament { get; set; }
    [ForeignKey(nameof(Prescription))]
    public int IdPrescription { get; set; }
    public int? Dose {get;set;}
    public string Details {get;set;}
    
    public Medicament Medicament {get;set;}
    public Prescription Prescription {get;set;}
}