using AppointmentSystem.DTOs.DoctorService;

namespace AppointmentSystem.Services.Interfaces
{
    public interface IDoctorServiceService
    {
        Task<IEnumerable<DoctorServiceResponseDto>> GetAllAsync();
        Task<IEnumerable<DoctorServiceResponseDto>> GetByDoctorIdAsync(int doctorId);
        Task<DoctorServiceResponseDto?> GetByIdAsync(int id);
        Task<DoctorServiceResponseDto> CreateAsync(CreateDoctorServiceDto dto);
        Task<bool> UpdateAsync(int id, UpdateDoctorServiceDto dto);
        Task<bool> DeleteAsync(int id);
    }
}