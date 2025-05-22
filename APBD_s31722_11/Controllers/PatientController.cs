using APBD_s31722_11.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APBD_s31722_11.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    
    private readonly IPrescriptionService _prescriptionService;

    public PatientController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetPatientWithPrescriptionById([FromRoute] int Id)
    {
        if (Id <= 0)
        {
            return BadRequest("Invalid patient ID");
        }
        try
        {
            var patientData = await _prescriptionService.GetPatientWithPrescriptionsAsync(Id);
            return Ok(patientData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}