using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<Appointment?> GetAppointmentWithDetailsAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsWithDetailsAsync();
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<bool> IsTimeSlotAvailableAsync(int doctorId, DateTime date, TimeOnly startTime, TimeOnly endTime, int? excludeAppointmentId = null);
    }
}