using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace APBD_s31722_11.DataLayer.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}