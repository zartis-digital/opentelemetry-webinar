using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Zartis.OpenTelemetry.VehicleServiceApi.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zartis.OpenTelemetry.VehicleServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly Services.VehicleService _vehicleService;

        public VehiclesController(Services.VehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Get()
        {
            var result = await Task.FromResult(new List<VehicleDto>()); // Simulating business call here
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<VehicleDto>> GetVehicle(string id)
        {
            var result = await _vehicleService.GetFullInformationFromSystemAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<VehicleDto>> Post(VehicleDto newVehicle) // Simulating business call here
        {
            var result = await Task.FromResult(newVehicle);
            return CreatedAtAction(nameof(GetVehicle), new { id = result.Vin }, result);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<VehicleDto>> Put(string id, VehicleDto modifiedVehicle)
        {
            var result = await Task.FromResult(modifiedVehicle); // Simulating business call here
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(string id)
        {
            await Task.CompletedTask; // Simulating business call here
            return Ok();
        }
    }
}
