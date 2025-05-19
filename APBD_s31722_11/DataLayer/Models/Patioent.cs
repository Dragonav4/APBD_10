using System.ComponentModel.DataAnnotations;

namespace APBD_s31722_11.DataLayer.Models;

public class Patient
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}