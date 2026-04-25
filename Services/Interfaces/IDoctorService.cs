using AppointmentSystem.DTOs.Doctor;

namespace AppointmentSystem.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorResponseDto>> GetAllDoctorsAsync();
        Task<DoctorResponseDto?> GetDoctorByIdAsync(int id);
        Task<DoctorResponseDto> CreateDoctorAsync(CreateDoctorDto dto);
        Task<bool> UpdateDoctorAsync(int id, UpdateDoctorDto dto);
        Task<bool> DeleteDoctorAsync(int id);
    }
}