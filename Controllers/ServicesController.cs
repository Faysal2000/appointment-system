using AppointmentSystem.DTOs.Service;
using AppointmentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _serviceService.GetAllAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null) return NotFound(new { message = "Service not found" });
            return Ok(service);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateServiceDto dto)
        {
            try
            {
                var service = await _serviceService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceDto dto)
        {
            var result = await _serviceService.UpdateAsync(id, dto);
            if (!result) return NotFound(new { message = "Service not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serviceService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Service not found" });
            return NoContent();
        }
    }
}