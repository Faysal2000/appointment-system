using AppointmentSystem.DTOs.Specialty;

namespace AppointmentSystem.Services.Interfaces
{
    public interface ISpecialtyService
    {
        Task<IEnumerable<SpecialtyResponseDto>> GetAllAsync();
        Task<SpecialtyResponseDto?> GetByIdAsync(int id);
        Task<SpecialtyResponseDto> CreateAsync(CreateSpecialtyDto dto);
        Task<bool> UpdateAsync(int id, UpdateSpecialtyDto dto);
        Task<bool> DeleteAsync(int id);
    }
}