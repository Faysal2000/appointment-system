using AppointmentSystem.DTOs.DoctorService;
using AppointmentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DoctorServicesController : ControllerBase
    {
        private readonly IDoctorServiceService _doctorServiceService;

        public DoctorServicesController(IDoctorServiceService doctorServiceService)
        {
            _doctorServiceService = doctorServiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _doctorServiceService.GetAllAsync();
            return Ok(services);
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctorId(int doctorId)
        {
            var services = await _doctorServiceService.GetByDoctorIdAsync(doctorId);
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _doctorServiceService.GetByIdAsync(id);
            if (service == null) return NotFound(new { message = "DoctorService not found" });
            return Ok(service);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateDoctorServiceDto dto)
        {
            try
            {
                var service = await _doctorServiceService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorServiceDto dto)
        {
            var result = await _doctorServiceService.UpdateAsync(id, dto);
            if (!result) return NotFound(new { message = "DoctorService not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _doctorServiceService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "DoctorService not found" });
            return NoContent();
        }
    }
}