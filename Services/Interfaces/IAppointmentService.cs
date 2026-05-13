using AppointmentSystem.DTOs.Appointment;

namespace AppointmentSystem.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();
        Task<AppointmentResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentResponseDto>> GetByDoctorIdAsync(int doctorId);
        Task<IEnumerable<AppointmentResponseDto>> GetByPatientIdAsync(int patientId);
        Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto);
        Task<bool> UpdateAsync(int id, UpdateAppointmentDto dto);
        Task<bool> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto);
        Task<bool> DeleteAsync(int id);
    }
}