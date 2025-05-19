using System;
using System.Threading.Tasks;
using APBD_s31722_11.Dto;
using APBD_s31722_11.Interfaces;
using APBD_s31722_11.Services;
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
                    value: new { IdPrescription = newId }
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}