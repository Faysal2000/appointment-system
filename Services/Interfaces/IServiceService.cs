using AppointmentSystem.DTOs.Service;

namespace AppointmentSystem.Services.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceResponseDto>> GetAllAsync();
        Task<ServiceResponseDto?> GetByIdAsync(int id);
        Task<ServiceResponseDto> CreateAsync(CreateServiceDto dto);
        Task<bool> UpdateAsync(int id, UpdateServiceDto dto);
        Task<bool> DeleteAsync(int id);
    }
}