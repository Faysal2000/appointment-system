using AppointmentSystem.Models;

namespace AppointmentSystem.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IGenericRepository<DoctorSpecialty> DoctorSpecialties { get; }

        IDoctorRepository Doctors { get; }
        IGenericRepository<Patient> Patients { get; }
        IAppointmentRepository Appointments { get; }
        IGenericRepository<Invoice> Invoices { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<Service> Services { get; }
        IGenericRepository<Specialty> Specialties { get; }
        IGenericRepository<DoctorService> DoctorServices { get; }
        IGenericRepository<DoctorSchedule> DoctorSchedules { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Role> Roles { get; }
        Task<int> SaveChangesAsync();
    }
}