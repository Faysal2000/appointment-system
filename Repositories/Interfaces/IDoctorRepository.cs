using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<Doctor?> GetDoctorWithDetailsAsync(int id);
        Task<IEnumerable<Doctor>> GetAllDoctorsWithDetailsAsync();
    }
}