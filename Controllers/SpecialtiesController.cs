using AppointmentSystem.DTOs.Specialty;
using AppointmentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtiesController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var specialties = await _specialtyService.GetAllAsync();
            return Ok(specialties);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var specialty = await _specialtyService.GetByIdAsync(id);
            if (specialty == null) return NotFound(new { message = "Specialty not found" });
            return Ok(specialty);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSpecialtyDto dto)
        {
            try
            {
                var specialty = await _specialtyService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = specialty.Id }, specialty);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecialtyDto dto)
        {
            var result = await _specialtyService.UpdateAsync(id, dto);
            if (!result) return NotFound(new { message = "Specialty not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _specialtyService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Specialty not found" });
            return NoContent();
        }
    }
}