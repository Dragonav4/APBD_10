using APBD_s31722_11.DataLayer.Models;
using APBD_s31722_11.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APBD_s31722_11.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PrescriptionRequestDto request)
        {
            try
            {
                var newId = await _prescriptionService.AddPrescriptionAsync(request);
                return CreatedAtAction(
                    actionName: nameof(Add),
                    routeValues: new { id = newId },
                    value: new PrescriptionCreatedDto() { IdPrescription = newId } // added after new ' PrescriptionDto' for test
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("patient/{Id}")]
        public async Task<IActionResult> GetPatientWithPrescriptionById([FromRoute] int Id)
        {
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
}